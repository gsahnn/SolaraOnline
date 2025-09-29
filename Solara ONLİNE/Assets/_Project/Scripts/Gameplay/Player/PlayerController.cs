using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(QuestLog))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 20f;

    private CharacterController controller;
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private PlayerCombatController playerCombat;
    private Camera mainCamera;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        playerCombat = GetComponent<PlayerCombatController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (IsAnyUIPanelOpen())
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        HandleRotation(); // Dönüþ her zaman aktif

        if (!isAttacking)
        {
            HandleMovement(); // Hareket sadece saldýrmýyorsa
        }

        HandleInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        animator.SetFloat("Speed", direction.magnitude);

        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        bool isMoving = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).magnitude >= 0.1f;

        if (playerTargeting.currentTarget != null)
        {
            Vector3 directionToTarget = playerTargeting.currentTarget.position - transform.position;
            directionToTarget.y = 0;
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
        else if (isMoving) // Sadece hedef yoksa VE hareket ediyorsa, gittiði yöne dönsün.
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleInput()
    {
        playerTargeting.HandleTargetingInput();

        if (Input.GetKey(KeyCode.Space)) playerCombat.SetAttackingState(true);
        if (Input.GetKeyUp(KeyCode.Space)) playerCombat.SetAttackingState(false);

        if (Input.GetKeyDown(KeyCode.C)) CharacterStatsUI_Controller.Instance?.TogglePanel();
    }

    private bool IsAnyUIPanelOpen()
    {
        return (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) ||
               (ShopSystem.Instance != null && ShopSystem.Instance.IsOpen());
    }
}