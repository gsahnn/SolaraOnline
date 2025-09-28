using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
// Diðer gerekli bileþenler...
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
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        playerCombat = GetComponent<PlayerCombatController>();
    }

    private void Update()
    {
        // UI paneli açýk mý kontrolü
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f); // Hareket animasyonunu durdur
            return;
        }

        // Karakterin saldýrýp saldýrmadýðýný kontrol et
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Saldýrý sýrasýnda hareketi engelle
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Girdileri dinle
        HandleInput();
    }

    // Karakterin fiziksel hareketini ve yürüme/durma animasyonunu yönetir
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

        // --- BLEND TREE OLMADAN HIZ KONTROLÜ ---
        // Animator'deki 'Speed' parametresine, karakterin hareket edip etmediðine göre
        // 0'dan büyük bir deðer veya 0 yazýyoruz. Animator, bu deðere göre
        // Wait -> Run veya Run -> Wait geçiþini kendisi yapacak.
        animator.SetFloat("Speed", moveDirection.magnitude);
        // ------------------------------------------

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // Oyuncunun klavye ve fare girdilerini dinler
    private void HandleInput()
    {
        playerTargeting.HandleTargetingInput();

        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        if (!isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerCombat.HandleAttackInput();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterStatsUI_Controller.Instance?.TogglePanel();
        }
    }

    // Hangi UI panelinin açýk olduðunu kontrol eden yardýmcý fonksiyon
    private bool IsAnyUIPanelOpen()
    {
        return CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
    }
}