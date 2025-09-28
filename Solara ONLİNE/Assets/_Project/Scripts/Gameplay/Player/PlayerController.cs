using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
// Di�er gerekli bile�enler...
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
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        playerCombat = GetComponent<PlayerCombatController>();
    }

    private void Update()
    {
        // UI paneli a��k m� kontrol�
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f); // Hareket animasyonunu durdur
            return;
        }

        // Karakterin sald�r�p sald�rmad���n� kontrol et
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Sald�r� s�ras�nda hareketi engelle
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Girdileri dinle
        HandleInput();
    }

    // Karakterin fiziksel hareketini ve y�r�me/durma animasyonunu y�netir
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

        // --- BLEND TREE OLMADAN HIZ KONTROL� ---
        // Animator'deki 'Speed' parametresine, karakterin hareket edip etmedi�ine g�re
        // 0'dan b�y�k bir de�er veya 0 yaz�yoruz. Animator, bu de�ere g�re
        // Wait -> Run veya Run -> Wait ge�i�ini kendisi yapacak.
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

    // Hangi UI panelinin a��k oldu�unu kontrol eden yard�mc� fonksiyon
    private bool IsAnyUIPanelOpen()
    {
        return CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
    }
}