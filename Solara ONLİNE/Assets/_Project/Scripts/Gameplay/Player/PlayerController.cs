using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
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
        // Eðer bir UI paneli açýksa, karakter kontrolünü tamamen durdur.
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Saldýrý durumunu Animator'den öðren.
        bool isAttacking = animator.GetBool("IsAttacking");

        // Saldýrý sýrasýnda hareketi engelle.
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Oyuncunun girdilerini dinle.
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

        // Boþluk tuþuna BASILDIÐI AN, saldýrý durumunu baþlat.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCombat.SetAttackingState(true);

        }

        // Boþluk tuþu BIRAKILDIÐI AN, saldýrý durumunu bitir.
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