// MonsterController.cs (Wait/Run sistemiyle tam uyumlu)
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Patrolling,
    Chasing,
    Attacking
}

[RequireComponent(typeof(CharacterStats), typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    [Header("Yapay Zeka Ayarlarý")]
    [SerializeField] private AIState currentState = AIState.Patrolling;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float timeBetweenAttacks = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private CharacterStats myStats;
    private Transform playerTarget;
    private Animator animator;

    private Vector3 startPosition;
    private float attackCooldownTimer = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        playerTarget = FindFirstObjectByType<PlayerController>()?.transform;
        startPosition = transform.position;
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;

        UpdateMovementAndRotation();

        switch (currentState)
        {
            case AIState.Idle: UpdateIdleState(); break;
            case AIState.Patrolling: UpdatePatrollingState(); break;
            case AIState.Chasing: UpdateChasingState(); break;
            case AIState.Attacking: UpdateAttackingState(); break;
        }
    }

    private void UpdateMovementAndRotation()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Bu satýr, Animator'e hýzý bildirir. Animator de buna göre 'Wait' veya 'Run' animasyonuna geçer.
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed, 0.1f, Time.deltaTime);
    }

    private void UpdateAttackingState()
    {
        if (playerTarget == null || Vector3.Distance(transform.position, playerTarget.position) > attackRange)
        {
            ChangeState(AIState.Chasing);
            return;
        }

        agent.ResetPath();
        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed * 2);

        if (attackCooldownTimer <= 0)
        {
            animator.SetTrigger("Attack");
            // Bu hasar verme, animasyonun ortasýna bir event olarak eklenebilir.
            // Örneðin: public void AnimationEvent_MonsterDealDamage() { ... }
            playerTarget.GetComponent<CharacterStats>().TakeDamage(Random.Range(myStats.minDamage, myStats.maxDamage + 1), myStats);
            attackCooldownTimer = timeBetweenAttacks;
        }
    }

    #region Unchanged State Functions
    private void ChangeState(AIState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }
    private void UpdateIdleState()
    {
        if (CanSeePlayer()) ChangeState(AIState.Chasing);
    }
    private void UpdatePatrollingState()
    {
        if (CanSeePlayer()) { ChangeState(AIState.Chasing); return; }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 randomPoint = startPosition + Random.insideUnitSphere * patrolRadius;
            NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, 1);
            agent.SetDestination(hit.position);
        }
    }
    private void UpdateChasingState()
    {
        if (playerTarget == null || !CanSeePlayer())
        {
            agent.SetDestination(startPosition);
            ChangeState(AIState.Patrolling);
            return;
        }

        if (Vector3.Distance(transform.position, playerTarget.position) <= attackRange)
        {
            ChangeState(AIState.Attacking);
        }
        else
        {
            agent.SetDestination(playerTarget.position);
        }
    }
    private bool CanSeePlayer()
    {
        if (playerTarget == null) return false;
        return Vector3.Distance(transform.position, playerTarget.position) <= sightRange;
    }
    #endregion

    #region Unchanged Original Functions
    public void HandleDeath(CharacterStats killer) { /*...*/ }
    private void DropLoot() { /*...*/ }
    private void InstantiateLoot(ItemData itemDataToDrop) { /*...*/ }
    #endregion
}