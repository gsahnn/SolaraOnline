// PlayerController.cs (SON HALÝ - KOPYALA YAPIÞTIR)
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;
    private MonsterController currentTarget;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        mainCamera = Camera.main;
        StartCoroutine(InitializeUIRoutine());
    }

    private IEnumerator InitializeUIRoutine()
    {
        yield return null;

        PlayerHUD_Controller hud = FindFirstObjectByType<PlayerHUD_Controller>();
        if (hud != null) hud.InitializeHUD(myStats);
        else Debug.LogError("PlayerHUD_Controller bulunamadý!");

        if (CharacterStatsUI_Controller.Instance != null)
        {
            CharacterStatsUI_Controller.Instance.Initialize(myStats);
        }
        else
        {
            Debug.LogError("CharacterStatsUI_Controller.Instance bulunamadý!");
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleAttackInput();
        HandleUIInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        animator.SetBool("IsMoving", moveDirection.magnitude > 0.1f);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CharacterStatsUI_Controller.Instance != null)
            {
                CharacterStatsUI_Controller.Instance.TogglePanel();
            }
        }
    }

    private void HandleAttackInput()
    {
        if (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) return;

        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.TryGetComponent(out MonsterController monster))
                {
                    StartAttack(monster);
                }
            }
        }
    }

    private void StartAttack(MonsterController target)
    {
        currentTarget = target;
        transform.LookAt(target.transform.position);
        animator.SetTrigger("Attack");
    }

    public void AnimationEvent_DealDamage()
    {
        if (currentTarget != null && currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            targetStats.TakeDamage(damage, myStats);
        }

        currentTarget = null;
    }
}