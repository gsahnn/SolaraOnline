// MonsterController.cs (YENÝ YAPAY ZEKA VERSÝYONU)
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent için bu kütüphane gerekli

public enum AIState
{
    Idle,       // Boþta duruyor
    Patrolling, // Geziniyor
    Chasing,    // Oyuncuyu kovalýyor
    Attacking   // Oyuncuya saldýrýyor
}

[RequireComponent(typeof(CharacterStats), typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    [Header("Yapay Zeka Ayarlarý")]
    [SerializeField] private AIState currentState = AIState.Patrolling;
    [SerializeField] private float sightRange = 10f; // Oyuncuyu fark etme mesafesi
    [SerializeField] private float attackRange = 2f; // Saldýrýya baþlama mesafesi
    [SerializeField] private float patrolRadius = 15f; // Baþlangýç noktasýndan ne kadar uzaða gezinebilir
    [SerializeField] private float timeBetweenAttacks = 2f; // Saldýrýlar arasý bekleme süresi

    // Referanslar
    private NavMeshAgent agent;
    private CharacterStats myStats;
    private Transform playerTarget;
    private Animator animator;

    // Zamanlayýcýlar ve Durum
    private Vector3 startPosition;
    private float attackCooldownTimer = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        // Oyuncuyu bul. Gerçek bir oyunda bu daha dinamik olur, þimdilik basit tutalým.
        playerTarget = FindFirstObjectByType<PlayerController>()?.transform;
        startPosition = transform.position;
    }

    private void Update()
    {
        // Zamanlayýcýlarý güncelle
        if (attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;

        // Animatör'ü güncelle (NavMeshAgent'ýn hýzýna göre yürüme animasyonunu ayarla)
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        // O anki duruma göre ne yapacaðýna karar ver
        switch (currentState)
        {
            case AIState.Idle:
                UpdateIdleState();
                break;
            case AIState.Patrolling:
                UpdatePatrollingState();
                break;
            case AIState.Chasing:
                UpdateChasingState();
                break;
            case AIState.Attacking:
                UpdateAttackingState();
                break;
        }
    }

    private void ChangeState(AIState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    // --- DURUM GÜNCELLEME FONKSÝYONLARI ---

    private void UpdateIdleState()
    {
        if (CanSeePlayer()) ChangeState(AIState.Chasing);
        // Ýstersen burada bir süre sonra tekrar devriyeye dönmesini saðlayabilirsin.
    }

    private void UpdatePatrollingState()
    {
        if (CanSeePlayer())
        {
            ChangeState(AIState.Chasing);
            return;
        }

        // Eðer hedefine ulaþtýysa veya hiç hedefi yoksa, yeni bir hedef belirle.
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
            // Oyuncuyu kaybettiyse, devriyeye geri dön.
            agent.SetDestination(startPosition);
            ChangeState(AIState.Patrolling);
            return;
        }

        // Oyuncu saldýrý menziline girdi mi?
        if (Vector3.Distance(transform.position, playerTarget.position) <= attackRange)
        {
            ChangeState(AIState.Attacking);
        }
        else
        {
            // Girmedilse, takibe devam et.
            agent.SetDestination(playerTarget.position);
        }
    }

    private void UpdateAttackingState()
    {
        if (playerTarget == null || Vector3.Distance(transform.position, playerTarget.position) > attackRange)
        {
            // Oyuncu menzilden çýktýysa, takibe geri dön.
            ChangeState(AIState.Chasing);
            return;
        }

        // Saldýrýya devam et
        agent.ResetPath(); // Saldýrýrken yürümesin.
        transform.LookAt(playerTarget); // Oyuncuya dön.

        if (attackCooldownTimer <= 0)
        {
            // Saldýrý zamaný!
            animator.SetTrigger("Attack"); // Saldýrý animasyonunu tetikle

            // Animasyon Event'i ile hasar verme burada da kullanýlabilir veya basitçe hasar verilebilir.
            // Þimdilik basitçe hasar verelim:
            playerTarget.GetComponent<CharacterStats>().TakeDamage(Random.Range(myStats.minDamage, myStats.maxDamage + 1), myStats);

            attackCooldownTimer = timeBetweenAttacks; // Bekleme süresini baþlat.
        }
    }

    // --- YARDIMCI FONKSÝYONLAR ---

    private bool CanSeePlayer()
    {
        if (playerTarget == null) return false;
        return Vector3.Distance(transform.position, playerTarget.position) <= sightRange;
    }

    // Bu fonksiyonlar hala ayný kalacak.
    #region Unchanged Methods
    public void HandleDeath(CharacterStats killer) { /*...*/ }
    private void DropLoot() { /*...*/ }
    private void InstantiateLoot(ItemData itemDataToDrop) { /*...*/ }
    #endregion
}
