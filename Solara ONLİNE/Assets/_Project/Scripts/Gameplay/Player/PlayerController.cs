using UnityEngine;
using System.Collections;

// DOÐRU KULLANIM: Her bir zorunlu bileþen için ayrý bir [RequireComponent] satýrý.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(SkillHolder))]
[RequireComponent(typeof(QuestLog))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Savaþ Ayarlarý")]
    [SerializeField] private LayerMask attackableLayers;

    private bool isActionInProgress = false;

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;
    private SkillHolder skillHolder;
    private MonsterController currentTarget;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        skillHolder = GetComponent<SkillHolder>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null) Debug.LogError("KRÝTÝK HATA: Sahnede 'MainCamera' etiketli bir kamera bulunamadý!");

        AddTestContent();
        InitializeUserInterfaces();
    }

    private void Update()
    {
        bool isUIOpen = CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
        if (isUIOpen || isActionInProgress) return;

        HandleMovement();
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { AttemptToUseSkill(0); return; }
        if (Input.GetMouseButtonDown(0)) { AttemptToAttack(); }
        if (Input.GetKeyDown(KeyCode.C)) { CharacterStatsUI_Controller.Instance?.TogglePanel(); }
    }

    private void AttemptToAttack()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, attackableLayers))
        {
            if (hit.collider.TryGetComponent(out MonsterController monster)) StartAttack(monster);
        }
    }

    private void StartAttack(MonsterController target)
    {
        currentTarget = target;
        transform.LookAt(target.transform.position);
        animator.SetTrigger("Attack");
        isActionInProgress = true;
    }

    public void AnimationEvent_DealDamage()
    {
        if (currentTarget != null && currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            targetStats.TakeDamage(damage, myStats);
        }
    }

    public void AnimationEvent_ActionFinished()
    {
        isActionInProgress = false;
        currentTarget = null;
    }

    private void InitializeUserInterfaces()
    {
        PlayerHUD_Controller.Instance?.InitializeHUD(myStats);
        CharacterStatsUI_Controller.Instance?.Initialize(myStats);
        SkillBar_Controller.Instance?.Initialize(skillHolder);
        QuestTracker_Controller.Instance?.Initialize(GetComponent<QuestLog>());
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        animator.SetBool("IsMoving", moveDirection.magnitude > 0.1f);
        if (moveDirection != Vector3.zero) { transform.rotation = Quaternion.LookRotation(moveDirection); }
    }

    private void AddTestContent()
    {
        skillHolder.LearnSkill(Resources.Load<SkillData>("Data/Skills/Güçlü Vuruþ"));
        GetComponent<QuestLog>().AddQuest(Resources.Load<QuestData>("Data/Quests/Kurt Avý"));
    }

    private void AttemptToUseSkill(int skillIndex)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, attackableLayers))
        {
            if (hit.collider.TryGetComponent(out MonsterController monster))
            {
                transform.LookAt(monster.transform);
                skillHolder.UseSkill(skillIndex, monster);
            }
        }
    }
}