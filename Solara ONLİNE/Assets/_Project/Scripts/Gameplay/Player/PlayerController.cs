using UnityEngine;

// Gerekli tüm bileþenlerin oyuncu objesinde olmasýný zorunlu kýlar.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(QuestLog))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    // Sistem Referanslarý
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
            // Hareket ediyormuþ gibi görünmemesi için hýzý animatörde sýfýrla.
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Saldýrý durumunu Animator'deki "Action" tag'i ile kontrol et.
        bool isPerformingAction = animator.GetCurrentAnimatorStateInfo(0).IsTag("Action");

        // Sadece bir eylem yapmýyorsa hareket etmesine izin ver.
        if (!isPerformingAction)
        {
            HandleMovement();
        }

        // Oyuncunun girdilerini her zaman dinle.
        HandleInput();
    }

    // Karakterin fiziksel hareketini ve yürüme/durma animasyonunu yönetir.
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

        // Animator'deki 'Speed' parametresini güncelle.
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Karakterin hareket yönüne dönmesini saðla.
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // Oyuncunun klavye ve fare girdilerini dinler ve ilgili sistemlere yönlendirir.
    private void HandleInput()
    {
        // Hedefleme her zaman çalýþýr, çünkü bir eylem sýrasýnda bile yeni bir hedef seçebiliriz.
        playerTargeting.HandleTargetingInput();

        // Boþluk tuþuna basýldýðýnda saldýrý komutunu PlayerCombatController'a gönder.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCombat.HandleAttackInput();
        }

        // UI panellerini aç/kapat.
        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterStatsUI_Controller.Instance?.TogglePanel();
        }

        // Kombo türünü deðiþtirmek için test tuþu (isteðe baðlý).
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerCombat.CycleNextComboType();
        }
    }

    // Hangi UI panelinin açýk olduðunu kontrol eden yardýmcý fonksiyon.
    private bool IsAnyUIPanelOpen()
    {
        // Bu liste, gelecekte yeni paneller eklendikçe geniþletilebilir.
        if (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) return true;
        if (ShopSystem.Instance != null && ShopSystem.Instance.IsOpen()) return true;

        return false;
    }
}