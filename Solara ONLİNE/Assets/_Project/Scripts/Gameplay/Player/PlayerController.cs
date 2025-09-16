using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(SkillHolder))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    // Durum Deðiþkeni
    private bool isAttacking = false;

    // Bileþen Referanslarý
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
        mainCamera = Camera.main;
    }

    private void Start()
    {
        AddTestSkills();
        InitializeUserInterfaces();
    }

    private void Update()
    {
        if (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen())
        {
            return;
        }

        if (isAttacking)
        {
            return;
        }

        HandleMovement();
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { AttemptToUseSkill(0); return; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { AttemptToUseSkill(1); return; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { AttemptToUseSkill(2); return; }

        if (Input.GetMouseButtonDown(0))
        {
            AttemptToAttack();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CharacterStatsUI_Controller.Instance != null) { CharacterStatsUI_Controller.Instance.TogglePanel(); }
        }
    }

    private void AttemptToAttack()
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

    private void AttemptToUseSkill(int skillIndex)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.TryGetComponent(out MonsterController monster))
            {
                transform.LookAt(monster.transform);
                skillHolder.UseSkill(skillIndex, monster);
            }
        }
    }

    private void StartAttack(MonsterController target)
    {
        currentTarget = target;
        transform.LookAt(target.transform.position);
        animator.SetTrigger("Attack");
        isAttacking = true;
    }

    public void AnimationEvent_DealDamage()
    {
        if (currentTarget != null && currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            targetStats.TakeDamage(damage, myStats);
        }
    }

    public void AnimationEvent_AttackFinished()
    {
        isAttacking = false;
        currentTarget = null;
    }

    private void InitializeUserInterfaces()
    {
        if (PlayerHUD_Controller.Instance != null)
            PlayerHUD_Controller.Instance.InitializeHUD(myStats);
        else Debug.LogError("PlayerHUD_Controller.Instance bulunamadý!");

        if (CharacterStatsUI_Controller.Instance != null)
            CharacterStatsUI_Controller.Instance.Initialize(myStats);
        else Debug.LogError("CharacterStatsUI_Controller.Instance bulunamadý!");

        if (SkillBar_Controller.Instance != null)
            SkillBar_Controller.Instance.Initialize(skillHolder);
        else Debug.LogError("SkillBar_Controller.Instance bulunamadý!");
    }

    private void AddTestSkills()
    {
        SkillData testSkill = Resources.Load<SkillData>("Data/Skills/Güçlü Vuruþ");
        if (testSkill != null)
        {
            skillHolder.LearnSkill(testSkill);
            Debug.Log(testSkill.skillName + " yeteneði test için öðrenildi.");
        }
        else
        {
            Debug.LogError("'Güçlü Vuruþ' yeteneði 'Resources/Data/Skills/' klasöründe bulunamadý! Yolu ve dosya adýný kontrol et.");
        }
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
}