using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq; // FirstOrDefault i�in

public enum AITemperament
{
    Passive,
    Aggressive
}

public enum AIState
{
    Patrolling,
    Chasing,
    Attacking,
    Dead
}

[RequireComponent(typeof(CharacterStats), typeof(NavMeshAgent), typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    [Header("Yapay Zeka Ayarlar�")]
    [SerializeField] private AITemperament temperament = AITemperament.Aggressive;
    [SerializeField] private AIState currentState = AIState.Patrolling;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float timeBetweenAttacks = 2f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Loot Ayarlar�")]
    [SerializeField] private LootTable lootTable;
    [SerializeField] private GameObject itemPickupPrefab;

    private NavMeshAgent agent;
    private CharacterStats myStats;
    private Transform playerTarget;
    private Animator animator;

    private Vector3 startPosition;
    private float attackCooldownTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        playerTarget = FindFirstObjectByType<PlayerController>()?.transform;

        agent.updateRotation = false;

        myStats.OnDeath += HandleDeath;
        myStats.OnDamageTaken += OnDamageTaken;
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnDestroy()
    {
        if (myStats != null)
        {
            myStats.OnDeath -= HandleDeath;
            myStats.OnDamageTaken -= OnDamageTaken;
        }
    }

    private void Update()
    {
        if (currentState == AIState.Dead) return;

        if (attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;

        UpdateMovementAndRotation();

        switch (currentState)
        {
            case AIState.Patrolling: UpdatePatrollingState(); break;
            case AIState.Chasing: UpdateChasingState(); break;
            case AIState.Attacking: UpdateAttackingState(); break;
        }
    }

    private void UpdateMovementAndRotation()
    {
        if (agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed, 0.1f, Time.deltaTime);
    }

    private void ChangeState(AIState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private void UpdatePatrollingState()
    {
        if (temperament == AITemperament.Aggressive && CanSeePlayer())
        {
            ChangeState(AIState.Chasing);
            return;
        }

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

    private void UpdateAttackingState()
    {
        if (playerTarget == null || Vector3.Distance(transform.position, playerTarget.position) > attackRange + 0.5f)
        {
            ChangeState(AIState.Chasing);
            return;
        }

        agent.SetDestination(transform.position);

        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (attackCooldownTimer <= 0)
        {
            animator.SetTrigger("Attack");
            attackCooldownTimer = timeBetweenAttacks;
        }
    }

    public void AnimationEvent_MonsterDealDamage()
    {
        if (playerTarget != null && Vector3.Distance(transform.position, playerTarget.position) <= attackRange + 0.5f)
        {
            playerTarget.GetComponent<CharacterStats>().TakeDamage(Random.Range(myStats.minDamage, myStats.maxDamage + 1), myStats);
        }
    }

    private void OnDamageTaken()
    {
        if (currentState != AIState.Dead)
        {
            ChangeState(AIState.Chasing);
            if (currentState != AIState.Attacking) animator.SetTrigger("Damage");
        }
    }

    private void HandleDeath(CharacterStats killer)
    {
        if (currentState == AIState.Dead) return;
        ChangeState(AIState.Dead);

        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        animator.SetTrigger("Die");

        if (killer != null)
        {
            killer.AddExperience(myStats.experienceGranted);
            if (killer.TryGetComponent(out QuestLog questLog))
            {
                questLog.AddQuestProgress(this.gameObject.name, 1);
            }
        }
        DropLoot();
        StartCoroutine(DestroyAfterAnimation(5f));
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private bool CanSeePlayer()
    {
        if (playerTarget == null) return false;

        return Vector3.Distance(transform.position, playerTarget.position) <= sightRange;
    }

    private void DropLoot()
    {
        if (lootTable == null) return;
        foreach (var lootItem in lootTable.possibleLoot)
        {
            float randomChance = Random.Range(0f, 100f);
            if (randomChance <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemData);
            }
        }
    }

    private void InstantiateLoot(ItemData itemDataToDrop)
    {
        if (itemPickupPrefab != null)
        {
            GameObject droppedItemObject = Instantiate(itemPickupPrefab, transform.position, Quaternion.identity);
            ItemPickup pickupScript = droppedItemObject.GetComponent<ItemPickup>();
            pickupScript.Initialize(itemDataToDrop, 1);
        }
    }
}

