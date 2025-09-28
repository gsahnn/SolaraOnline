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
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Saldýrý durumunu Animator'den öðren.
        bool isAttacking = animator.GetBool("IsAttacking");

        if (!isAttacking)
        {
            HandleMovement();
        }

        HandleInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        animator.SetFloat("Speed", moveDirection.magnitude);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void HandleInput()
    {
        playerTargeting.HandleTargetingInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCombat.SetAttackingState(true);
        }

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