using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(CharacterStats), typeof(NavMeshAgent), typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    public enum AITemperament { Passive, Aggressive }
    public enum AIState { Idle, Patrolling, Chasing, Attacking, Dead }

    [Header("Yapay Zeka Ayarlarý")]
    [SerializeField] private AITemperament temperament = AITemperament.Aggressive;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float minIdleTime = 2f;
    [SerializeField] private float maxIdleTime = 5f;
    [SerializeField] private float timeBetweenAttacks = 2f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Loot Ayarlarý")]
    [SerializeField] private LootTable lootTable;
    [SerializeField] private GameObject itemPickupPrefab;

    private NavMeshAgent agent;
    private CharacterStats myStats;
    private Transform playerTarget;
    private Animator animator;
    private AIState currentState;
    private Vector3 startPosition;
    private float stateTimer = 0f;
    private float nextActionTime = 0f;
    private float attackCooldownTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        // Player'ý bulmanýn en güvenli yolu PlayerManager veya benzeri bir singleton'dýr.
        // Þimdilik bu þekilde býrakýyoruz.
        playerTarget = FindFirstObjectByType<PlayerController>()?.transform;

        // Rotasyon kontrolünü NavMeshAgent'tan alýp kendi kodumuza veriyoruz.
        agent.updateRotation = false;

        // Event'lere abone oluyoruz.
        myStats.OnDeath += HandleDeath;
        myStats.OnDamageTaken += OnDamageTaken;
    }

    private void Start()
    {
        startPosition = transform.position;
        ChangeState(AIState.Idle);
    }

    private void OnDestroy()
    {
        // Hafýza sýzýntýlarýný önlemek için event'lerden aboneliði kaldýrýyoruz.
        if (myStats != null)
        {
            myStats.OnDeath -= HandleDeath;
            myStats.OnDamageTaken -= OnDamageTaken;
        }
    }

    private void Update()
    {
        if (currentState == AIState.Dead) return;
        stateTimer += Time.deltaTime;

        // Her frame'de, durum ne olursa olsun, harekete göre animasyonu ve dönüþü güncelle.
        UpdateMovementAndRotation();

        switch (currentState)
        {
            case AIState.Idle: UpdateIdleState(); break;
            case AIState.Patrolling: UpdatePatrollingState(); break;
            case AIState.Chasing: UpdateChasingState(); break;
            case AIState.Attacking: UpdateAttackingState(); break;
        }
    }

    private void ChangeState(AIState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        stateTimer = 0f;

        if (newState == AIState.Idle)
        {
            agent.ResetPath();
            nextActionTime = Random.Range(minIdleTime, maxIdleTime);
            animator.SetFloat("IdleVariation", (float)Random.Range(0, 3)); // Idle çeþitliliði için
        }
    }

    // --- AÞAÐIDAKÝ 3 FONKSÝYON, NÝHAÝ DÜZELTMELERÝ ÝÇERÝR ---

    private void UpdateMovementAndRotation()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        // Eðer NavMeshAgent'ý hareket ettiriyorsa, gittiði yöne doðru dön.
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
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
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = timeBetweenAttacks;
            animator.SetTrigger("Attack");
        }
    }

    public void AnimationEvent_MonsterDealDamage()
    {
        if (playerTarget != null && Vector3.Distance(transform.position, playerTarget.position) <= attackRange + 0.5f)
        {
            CharacterStats targetStats = playerTarget.GetComponent<CharacterStats>();
            if (targetStats == null) return;

            // DÜZELTME: TakeDamage fonksiyonunu 3 parametre ile doðru bir þekilde çaðýrýyoruz.
            // Canavarýn normal vuruþlarý yere düþürmediði için 'false' gönderiyoruz.
            targetStats.TakeDamage(Random.Range(myStats.minDamage, myStats.maxDamage + 1), myStats, false);
        }
    }

    #region Mevcut AI Fonksiyonlarý (DEÐÝÞÝKLÝK YOK)
    private void UpdateIdleState()
    {
        if (temperament == AITemperament.Aggressive && CanSeePlayer()) { ChangeState(AIState.Chasing); return; }
        if (stateTimer > nextActionTime)
        {
            ChangeState(AIState.Patrolling);
            GoToNewPatrolPoint();
        }
    }

    private void UpdatePatrollingState()
    {
        if (temperament == AITemperament.Aggressive && CanSeePlayer()) { ChangeState(AIState.Chasing); return; }
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.1f)
        {
            ChangeState(AIState.Idle);
        }
    }

    private void GoToNewPatrolPoint()
    {
        Vector3 randomPoint = startPosition + Random.insideUnitSphere * patrolRadius;
        NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }

    private void UpdateChasingState()
    {
        if (playerTarget == null || !CanSeePlayer()) { ChangeState(AIState.Patrolling); return; }

        if (Vector3.Distance(transform.position, playerTarget.position) <= agent.stoppingDistance)
        {
            ChangeState(AIState.Attacking);
        }
        else
        {
            agent.SetDestination(playerTarget.position);
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
                if (Mathf.Abs(killer.level - myStats.level) <= 10)
                {
                    questLog.AddQuestProgress(this.gameObject.name, 1);
                    killer.ChangeAlignment(5);
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
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }
    #endregion
}