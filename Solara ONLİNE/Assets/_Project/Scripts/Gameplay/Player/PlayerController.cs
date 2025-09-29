using UnityEngine;

// Gerekli t�m bile�enlerin oyuncu objesinde olmas�n� zorunlu k�lar.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(QuestLog))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    // Sistem Referanslar�
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
            // Hareket ediyormu� gibi g�r�nmemesi i�in h�z� animat�rde s�f�rla.
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Sald�r� durumunu Animator'deki "Action" tag'i ile kontrol et.
        bool isPerformingAction = animator.GetCurrentAnimatorStateInfo(0).IsTag("Action");

        // Sadece bir eylem yapm�yorsa hareket etmesine izin ver.
        if (!isPerformingAction)
        {
            HandleMovement();
        }

        // Oyuncunun girdilerini her zaman dinle.
        HandleInput();
    }

    // Karakterin fiziksel hareketini ve y�r�me/durma animasyonunu y�netir.
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

        // Animator'deki 'Speed' parametresini g�ncelle.
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Karakterin hareket y�n�ne d�nmesini sa�la.
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // Oyuncunun klavye ve fare girdilerini dinler ve ilgili sistemlere y�nlendirir.
    private void HandleInput()
    {
        // Hedefleme her zaman �al���r, ��nk� bir eylem s�ras�nda bile yeni bir hedef se�ebiliriz.
        playerTargeting.HandleTargetingInput();

        // Bo�luk tu�una bas�ld���nda sald�r� komutunu PlayerCombatController'a g�nder.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCombat.HandleAttackInput();
        }

        // UI panellerini a�/kapat.
        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterStatsUI_Controller.Instance?.TogglePanel();
        }

        // Kombo t�r�n� de�i�tirmek i�in test tu�u (iste�e ba�l�).
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerCombat.CycleNextComboType();
        }
    }

    // Hangi UI panelinin a��k oldu�unu kontrol eden yard�mc� fonksiyon.
    private bool IsAnyUIPanelOpen()
    {
        // Bu liste, gelecekte yeni paneller eklendik�e geni�letilebilir.
        if (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) return true;
        if (ShopSystem.Instance != null && ShopSystem.Instance.IsOpen()) return true;

        return false;
    }
}