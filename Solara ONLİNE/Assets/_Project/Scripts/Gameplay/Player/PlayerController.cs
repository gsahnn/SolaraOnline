using UnityEngine;

// Gerekli tüm bileþenlerin oyuncu objesinde olmasýný zorunlu kýlar.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    // Diðer sistemlere referanslar
    private CharacterController controller;
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private PlayerCombatController playerCombat;

    private void Awake()
    {
        // Oyun baþlarken gerekli tüm bileþenlere referanslarý al.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        playerCombat = GetComponent<PlayerCombatController>();
    }

    private void Update()
    {
        // Eðer bir UI paneli (Karakter, Envanter, Dükkan vb.) açýksa, oyuncu kontrolünü tamamen devre dýþý býrak.
        if (IsAnyUIPanelOpen())
        {
            // Hareket animasyonunun takýlý kalmamasý için hýzý sýfýrla.
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Animator'deki mevcut durumun etiketini kontrol et.
        // Tek Layer sisteminde, bu Base Layer (index 0) olacaktýr.
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Saldýrý sýrasýnda hareketi engelle
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Girdileri dinle
        HandleInput();
    }

    // Karakterin fiziksel hareketini ve yürüme/koþma animasyonunu yönetir.
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

    // Oyuncunun klavye ve fare girdilerini dinler ve ilgili sistemlere yönlendirir.
    private void HandleInput()
    {
        // Hedefleme her zaman çalýþýr, çünkü saldýrý animasyonu sýrasýnda bile yeni bir hedef seçebiliriz.
        playerTargeting.HandleTargetingInput();

        // Eðer saldýrý animasyonu oynamýyorsa, yeni bir eylem komutu alabiliriz.
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerCombat.HandleAttackInput();
            }

            // ... Diðer yetenek tuþlarý (Alpha1, Alpha2 vb.) buraya eklenebilir ...
            // if (Input.GetKeyDown(KeyCode.Alpha1)) { ... }
        }

        // UI panelleri her zaman açýlýp kapanabilmeli.
        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterStatsUI_Controller.Instance?.TogglePanel();
        }
    }

    // Hangi UI panelinin açýk olduðunu kontrol eden yardýmcý fonksiyon.
    private bool IsAnyUIPanelOpen()
    {
        // Ýleride buraya Envanter, Yetenekler, Dükkan panellerinin kontrolü de eklenecek.
        // return CharacterStatsUI_Controller.Instance.IsOpen() || InventoryUI.Instance.IsOpen() ...
        return CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
    }
}