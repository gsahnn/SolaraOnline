using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(SkillHolder))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;
    private SkillHolder skillHolder;
    private MonsterController currentTarget;

    private void Awake()
    {
        // Referanslar� Awake i�inde almak, Start fonksiyonlar� aras�ndaki
        // zamanlama sorunlar�n� �nlemek i�in daha g�venli bir y�ntemdir.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        skillHolder = GetComponent<SkillHolder>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        // �nce karakterin temel verilerini haz�rla (�rne�in yeteneklerini ��ren)
        AddTestSkills();

        // Sonra, bu haz�r verilerle UI sistemlerini ba�lat.
        InitializeUserInterfaces();
        QuestData testQuest = Resources.Load<QuestData>("Data/Quests/Kurt Av�");
        if (testQuest != null) ;
        GetComponent<QuestLog>().AddQuest(testQuest);
    }

    private void AddTestSkills()
    {
        // Bu kodun �al��mas� i�in 'G��l� Vuru�' asset'inin yolu 'Assets/Resources/Data/Skills/' olmal�d�r.
        SkillData testSkill = Resources.Load<SkillData>("Data/Skills/G��l� Vuru�");
        if (testSkill != null)
        {
            skillHolder.LearnSkill(testSkill);
            Debug.Log(testSkill.skillName + " yetene�i test i�in ��renildi.");
        }
        else
        {
            Debug.LogError("'G��l� Vuru�' yetene�i 'Resources/Data/Skills/' klas�r�nde bulunamad�! Yolu ve dosya ad�n� kontrol et.");
        }
    }

    private void InitializeUserInterfaces()
    {
        // Singleton'lar haz�r oldu�u i�in do�rudan eri�im sa�l�yoruz.
        // Bu Coroutine'den daha basit ve g�venilir bir y�ntemdir.
        if (PlayerHUD_Controller.Instance != null)
            PlayerHUD_Controller.Instance.InitializeHUD(myStats);
        else Debug.LogError("PlayerHUD_Controller.Instance bulunamad�!");

        if (CharacterStatsUI_Controller.Instance != null)
            CharacterStatsUI_Controller.Instance.Initialize(myStats);
        else Debug.LogError("CharacterStatsUI_Controller.Instance bulunamad�!");

        if (SkillBar_Controller.Instance != null)
            SkillBar_Controller.Instance.Initialize(skillHolder);
        else Debug.LogError("SkillBar_Controller.Instance bulunamad�!");
    }

    private void Update()
    {
        HandleMovement();
        HandleAttackInput();
        HandleUIInput();
        HandleSkillInput();
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

    private void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CharacterStatsUI_Controller.Instance != null) { CharacterStatsUI_Controller.Instance.TogglePanel(); }
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
                if (hit.collider.TryGetComponent(out MonsterController monster)) { StartAttack(monster); }
            }
        }
    }

    private void HandleSkillInput()
    {
        if (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) { AttemptToUseSkill(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { AttemptToUseSkill(1); }
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