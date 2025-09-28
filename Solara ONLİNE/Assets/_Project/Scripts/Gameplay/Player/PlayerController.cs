using UnityEngine;

// Gerekli t�m bile�enlerin oyuncu objesinde olmas�n� zorunlu k�lar.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    // Di�er sistemlere referanslar
    private CharacterController controller;
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private PlayerCombatController playerCombat;

    private void Awake()
    {
        // Oyun ba�larken gerekli t�m bile�enlere referanslar� al.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        playerCombat = GetComponent<PlayerCombatController>();
    }

    private void Update()
    {
        // E�er bir UI paneli (Karakter, Envanter, D�kkan vb.) a��ksa, oyuncu kontrol�n� tamamen devre d��� b�rak.
        if (IsAnyUIPanelOpen())
        {
            // Hareket animasyonunun tak�l� kalmamas� i�in h�z� s�f�rla.
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Animator'deki mevcut durumun etiketini kontrol et.
        // Tek Layer sisteminde, bu Base Layer (index 0) olacakt�r.
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Sald�r� s�ras�nda hareketi engelle
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Girdileri dinle
        HandleInput();
    }

    // Karakterin fiziksel hareketini ve y�r�me/ko�ma animasyonunu y�netir.
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        float currentSpeed = moveDirection.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // Oyuncunun klavye ve fare girdilerini dinler ve ilgili sistemlere y�nlendirir.
    private void HandleInput()
    {
        // Hedefleme her zaman �al���r, ��nk� sald�r� animasyonu s�ras�nda bile yeni bir hedef se�ebiliriz.
        playerTargeting.HandleTargetingInput();

        // E�er sald�r� animasyonu oynam�yorsa, yeni bir eylem komutu alabiliriz.
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerCombat.HandleAttackInput();
            }

            // ... Di�er yetenek tu�lar� (Alpha1, Alpha2 vb.) buraya eklenebilir ...
            // if (Input.GetKeyDown(KeyCode.Alpha1)) { ... }
        }

        // UI panelleri her zaman a��l�p kapanabilmeli.
        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterStatsUI_Controller.Instance?.TogglePanel();
        }
    }

    // Hangi UI panelinin a��k oldu�unu kontrol eden yard�mc� fonksiyon.
    private bool IsAnyUIPanelOpen()
    {
        // �leride buraya Envanter, Yetenekler, D�kkan panellerinin kontrol� de eklenecek.
        // return CharacterStatsUI_Controller.Instance.IsOpen() || InventoryUI.Instance.IsOpen() ...
        return CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
    }
}