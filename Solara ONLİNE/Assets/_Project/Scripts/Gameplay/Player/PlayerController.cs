using UnityEngine;

// Gerekli component'lerin Player objesinde olmasýný zorunlu kýlar.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(QuestLog))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 20f;

    // Gerekli component'ler için referanslar
    private CharacterController controller;
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private PlayerCombatController playerCombat;
    private Camera mainCamera;

    private void Awake()
    {
        // Baþlangýçta tüm component referanslarýný alýyoruz.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        playerCombat = GetComponent<PlayerCombatController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Eðer herhangi bir UI paneli açýksa, karakter kontrolünü devre dýþý býrak.
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f); // Karakterin koþma animasyonunu durdur.
            return;
        }

        // Karakterin mevcut animasyon durumunu kontrol et (saldýrýyor mu?)
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Karakterin dönüþünü her zaman kontrol et (hedefe kilitlenme).
        HandleRotation();

        // Sadece saldýrmýyorsa hareket etmesine izin ver.
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Diðer oyuncu girdilerini (hedefleme, saldýrma tuþu vb.) iþle.
        HandleInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Animator'deki "Speed" parametresini güncelleyerek yürüme/koþma animasyonunu tetikle.
        animator.SetFloat("Speed", direction.magnitude);

        if (direction.magnitude >= 0.1f)
        {
            // CharacterController'ý kullanarak karakteri hareket ettir.
            controller.Move(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        bool isMoving = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).magnitude >= 0.1f;

        // Eðer bir hedef varsa...
        if (playerTargeting.currentTarget != null)
        {
            // --- YAPILAN TEK KRÝTÝK DEÐÝÞÝKLÝK BURADA ---
            // Hedefin CharacterStats component'inden transform'una, oradan da position'ýna eriþiyoruz.
            Vector3 directionToTarget = playerTargeting.currentTarget.transform.position - transform.position;
            directionToTarget.y = 0; // Y ekseninde dönmesini engelle.
            transform.rotation = Quaternion.LookRotation(directionToTarget); // Anýnda hedefe dön.
        }
        // Eðer hedef yoksa VE karakter hareket ediyorsa...
        else if (isMoving)
        {
            // ...karakterin, hareket ettiði yöne doðru yavaþça dönmesini saðla.
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleInput()
    {
        // Hedefleme girdisini iþlemesi için PlayerTargeting script'ini bilgilendir.
        playerTargeting.HandleTargetingInput();

        // Boþluk tuþuna basýlý tutulursa saldýrý durumunu aktif et.
        if (Input.GetKey(KeyCode.Space)) playerCombat.SetAttackingState(true);
        if (Input.GetKeyUp(KeyCode.Space)) playerCombat.SetAttackingState(false);

        // 'C' tuþuna basýldýðýnda karakter stat panelini aç/kapat.
        if (Input.GetKeyDown(KeyCode.C)) CharacterStatsUI_Controller.Instance?.TogglePanel();
    }

    // Herhangi bir UI panelinin açýk olup olmadýðýný kontrol eden yardýmcý fonksiyon.
    private bool IsAnyUIPanelOpen()
    {
        return (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) ||
               (ShopSystem.Instance != null && ShopSystem.Instance.IsOpen());
        // Gelecekte buraya Envanter, Yetenekler vb. paneller de eklenebilir.
    }
}