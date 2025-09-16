// MonsterController.cs (YEN� YAPAY ZEKA VERS�YONU)
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent i�in bu k�t�phane gerekli

public enum AIState
{
    Idle,       // Bo�ta duruyor
    Patrolling, // Geziniyor
    Chasing,    // Oyuncuyu koval�yor
    Attacking   // Oyuncuya sald�r�yor
}

[RequireComponent(typeof(CharacterStats), typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    [Header("Yapay Zeka Ayarlar�")]
    [SerializeField] private AIState currentState = AIState.Patrolling;
    [SerializeField] private float sightRange = 10f; // Oyuncuyu fark etme mesafesi
    [SerializeField] private float attackRange = 2f; // Sald�r�ya ba�lama mesafesi
    [SerializeField] private float patrolRadius = 15f; // Ba�lang�� noktas�ndan ne kadar uza�a gezinebilir
    [SerializeField] private float timeBetweenAttacks = 2f; // Sald�r�lar aras� bekleme s�resi

    // Referanslar
    private NavMeshAgent agent;
    private CharacterStats myStats;
    private Transform playerTarget;
    private Animator animator;

    // Zamanlay�c�lar ve Durum
    private Vector3 startPosition;
    private float attackCooldownTimer = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        // Oyuncuyu bul. Ger�ek bir oyunda bu daha dinamik olur, �imdilik basit tutal�m.
        playerTarget = FindFirstObjectByType<PlayerController>()?.transform;
        startPosition = transform.position;
    }

    private void Update()
    {
        // Zamanlay�c�lar� g�ncelle
        if (attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;

        // Animat�r'� g�ncelle (NavMeshAgent'�n h�z�na g�re y�r�me animasyonunu ayarla)
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        // O anki duruma g�re ne yapaca��na karar ver
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

    // --- DURUM G�NCELLEME FONKS�YONLARI ---

    private void UpdateIdleState()
    {
        if (CanSeePlayer()) ChangeState(AIState.Chasing);
        // �stersen burada bir s�re sonra tekrar devriyeye d�nmesini sa�layabilirsin.
    }

    private void UpdatePatrollingState()
    {
        if (CanSeePlayer())
        {
            ChangeState(AIState.Chasing);
            return;
        }

        // E�er hedefine ula�t�ysa veya hi� hedefi yoksa, yeni bir hedef belirle.
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
            // Oyuncuyu kaybettiyse, devriyeye geri d�n.
            agent.SetDestination(startPosition);
            ChangeState(AIState.Patrolling);
            return;
        }

        // Oyuncu sald�r� menziline girdi mi?
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
            // Oyuncu menzilden ��kt�ysa, takibe geri d�n.
            ChangeState(AIState.Chasing);
            return;
        }

        // Sald�r�ya devam et
        agent.ResetPath(); // Sald�r�rken y�r�mesin.
        transform.LookAt(playerTarget); // Oyuncuya d�n.

        if (attackCooldownTimer <= 0)
        {
            // Sald�r� zaman�!
            animator.SetTrigger("Attack"); // Sald�r� animasyonunu tetikle

            // Animasyon Event'i ile hasar verme burada da kullan�labilir veya basit�e hasar verilebilir.
            // �imdilik basit�e hasar verelim:
            playerTarget.GetComponent<CharacterStats>().TakeDamage(Random.Range(myStats.minDamage, myStats.maxDamage + 1), myStats);

            attackCooldownTimer = timeBetweenAttacks; // Bekleme s�resini ba�lat.
        }
    }

    // --- YARDIMCI FONKS�YONLAR ---

    private bool CanSeePlayer()
    {
        if (playerTarget == null) return false;
        return Vector3.Distance(transform.position, playerTarget.position) <= sightRange;
    }

    // Bu fonksiyonlar hala ayn� kalacak.
    #region Unchanged Methods
    public void HandleDeath(CharacterStats killer) { /*...*/ }
    private void DropLoot() { /*...*/ }
    private void InstantiateLoot(ItemData itemDataToDrop) { /*...*/ }
    #endregion
}
