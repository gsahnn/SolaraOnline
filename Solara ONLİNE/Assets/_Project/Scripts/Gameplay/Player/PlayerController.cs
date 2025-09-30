using UnityEngine;

// Gerekli component'lerin Player objesinde olmas�n� zorunlu k�lar.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(QuestLog))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 20f;

    // Gerekli component'ler i�in referanslar
    private CharacterController controller;
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private PlayerCombatController playerCombat;
    private Camera mainCamera;

    private void Awake()
    {
        // Ba�lang��ta t�m component referanslar�n� al�yoruz.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        playerCombat = GetComponent<PlayerCombatController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // E�er herhangi bir UI paneli a��ksa, karakter kontrol�n� devre d��� b�rak.
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f); // Karakterin ko�ma animasyonunu durdur.
            return;
        }

        // Karakterin mevcut animasyon durumunu kontrol et (sald�r�yor mu?)
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Karakterin d�n���n� her zaman kontrol et (hedefe kilitlenme).
        HandleRotation();

        // Sadece sald�rm�yorsa hareket etmesine izin ver.
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Di�er oyuncu girdilerini (hedefleme, sald�rma tu�u vb.) i�le.
        HandleInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Animator'deki "Speed" parametresini g�ncelleyerek y�r�me/ko�ma animasyonunu tetikle.
        animator.SetFloat("Speed", direction.magnitude);

        if (direction.magnitude >= 0.1f)
        {
            // CharacterController'� kullanarak karakteri hareket ettir.
            controller.Move(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        bool isMoving = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).magnitude >= 0.1f;

        // E�er bir hedef varsa...
        if (playerTargeting.currentTarget != null)
        {
            // --- YAPILAN TEK KR�T�K DE����KL�K BURADA ---
            // Hedefin CharacterStats component'inden transform'una, oradan da position'�na eri�iyoruz.
            Vector3 directionToTarget = playerTargeting.currentTarget.transform.position - transform.position;
            directionToTarget.y = 0; // Y ekseninde d�nmesini engelle.
            transform.rotation = Quaternion.LookRotation(directionToTarget); // An�nda hedefe d�n.
        }
        // E�er hedef yoksa VE karakter hareket ediyorsa...
        else if (isMoving)
        {
            // ...karakterin, hareket etti�i y�ne do�ru yava��a d�nmesini sa�la.
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleInput()
    {
        // Hedefleme girdisini i�lemesi i�in PlayerTargeting script'ini bilgilendir.
        playerTargeting.HandleTargetingInput();

        // Bo�luk tu�una bas�l� tutulursa sald�r� durumunu aktif et.
        if (Input.GetKey(KeyCode.Space)) playerCombat.SetAttackingState(true);
        if (Input.GetKeyUp(KeyCode.Space)) playerCombat.SetAttackingState(false);

        // 'C' tu�una bas�ld���nda karakter stat panelini a�/kapat.
        if (Input.GetKeyDown(KeyCode.C)) CharacterStatsUI_Controller.Instance?.TogglePanel();
    }

    // Herhangi bir UI panelinin a��k olup olmad���n� kontrol eden yard�mc� fonksiyon.
    private bool IsAnyUIPanelOpen()
    {
        return (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) ||
               (ShopSystem.Instance != null && ShopSystem.Instance.IsOpen());
        // Gelecekte buraya Envanter, Yetenekler vb. paneller de eklenebilir.
    }
}