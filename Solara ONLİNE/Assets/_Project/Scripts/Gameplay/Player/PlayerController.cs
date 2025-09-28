using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
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
        // E�er bir UI paneli a��ksa, karakter kontrol�n� tamamen durdur.
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Sald�r� durumunu Animator'den ��ren.
        bool isAttacking = animator.GetBool("IsAttacking");

        // Sald�r� s�ras�nda hareketi engelle.
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Oyuncunun girdilerini dinle.
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

        animator.SetFloat("Speed", moveDirection.magnitude);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // Oyuncunun klavye ve fare girdilerini dinler.
    private void HandleInput()
    {
        playerTargeting.HandleTargetingInput();

        // Bo�luk tu�una BASILDI�I AN, sald�r� durumunu ba�lat.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCombat.SetAttackingState(true);

        }

        // Bo�luk tu�u BIRAKILDI�I AN, sald�r� durumunu bitir.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerCombat.SetAttackingState(false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterStatsUI_Controller.Instance?.TogglePanel();
        }
    }

    private bool IsAnyUIPanelOpen()
    {
        return CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
    }
}