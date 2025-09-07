// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    // Bileþen Referanslarý
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;

    // Savaþ Durum Deðiþkeni
    private MonsterController currentTarget; // Hedef hala MonsterController tipinde

    private void Start()
    {
        // Bileþen referanslarýný al.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        mainCamera = Camera.main;

        // HUD kontrolcüsünü bul ve kendini ona tanýt.
        PlayerHUD_Controller hud = FindFirstObjectByType<PlayerHUD_Controller>();
        if (hud != null)
        {
            hud.InitializeHUD(myStats);
        }
        else
        {
            // Eðer HUD bulunamazsa, bu önemli bir hatadýr. Konsolda belirtelim.
            Debug.LogError("Sahnede PlayerHUD_Controller bulunamadý! Manager_Scene'i kontrol et.");
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleAttackInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        animator.SetBool("IsMoving", moveDirection.magnitude > 0.1f);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.TryGetComponent(out MonsterController monster))
                {
                    StartAttack(monster);
                }
            }
        }
    }

    private void StartAttack(MonsterController target)
    {
        currentTarget = target;
        transform.LookAt(target.transform.position);
        animator.SetTrigger("Attack");
    }

    // Animasyon Event'i tarafýndan çaðrýlacak olan fonksiyon (DÜZELTÝLDÝ).
    public void AnimationEvent_DealDamage()
    {
        // Hedefimiz hala var mý ve hedefin bir CharacterStats bileþeni var mý?
        if (currentTarget != null && currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            // Rastgele bir hasar deðeri hesapla
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);

            // Hedefin CharacterStats bileþenindeki TakeDamage fonksiyonunu çaðýr.
            // Saldýranýn kim olduðunu da (bizim statlarýmýzý) parametre olarak gönder.
            targetStats.TakeDamage(damage, myStats);
        }

        currentTarget = null;
    }
}
