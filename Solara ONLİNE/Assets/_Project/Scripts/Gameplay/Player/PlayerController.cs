using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(SkillHolder))]
[RequireComponent(typeof(QuestLog))]
[RequireComponent(typeof(PlayerTargeting))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Savaþ Ayarlarý")]
    [SerializeField] private LayerMask attackableLayers;
    [SerializeField] private float comboResetTime = 1.5f;

    // Sistem Referanslarý
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;
    private SkillHolder skillHolder;
    private PlayerTargeting playerTargeting;
    private MonsterController currentTarget;

    // Kombo Sistemi Deðiþkenleri
    private int comboCounter = 0;
    private float lastAttackInputTime = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        skillHolder = GetComponent<SkillHolder>();
        playerTargeting = GetComponent<PlayerTargeting>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        AddTestContent();
        InitializeUserInterfaces();
    }

    private void Update()
    {
        // Kombo zaman aþýmýný kontrol et
        if (Time.time - lastAttackInputTime > comboResetTime)
        {
            ResetCombo();
        }

        bool isUIOpen = CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
        // Animator'deki durumun etiketini kontrol et. Bu en güvenilir yöntemdir.
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Eðer bir UI paneli açýksa, hiçbir þey yapma.
        if (isUIOpen) return;

        // Saldýrý sýrasýnda hareketi engelle
        if (!isAttacking)
        {
            HandleMovement();
        }

        // Girdi kontrolünü her zaman yap, içindeki fonksiyonlar kendi kontrollerini yapacak.
        HandleInput();
    }

    private void HandleInput()
    {
        // Eðer zaten saldýrýyorsak, yeni bir saldýrý veya yetenek komutu alma.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) { AttemptToUseSkill(0); return; }
        if (Input.GetKeyDown(KeyCode.Space)) { AttemptToAttack(); }
        if (Input.GetKeyDown(KeyCode.C)) { CharacterStatsUI_Controller.Instance?.TogglePanel(); }
    }

    private void AttemptToAttack()
    {
        if (playerTargeting.currentTarget != null && playerTargeting.currentTarget.TryGetComponent(out MonsterController monster))
        {
            StartAttack(monster);
        }
        else
        {
            // Ýleride buraya en yakýn düþmaný bulma mantýðý eklenebilir.
        }
    }

    private void StartAttack(MonsterController target)
    {
        currentTarget = target;
        transform.LookAt(target.transform.position);

        lastAttackInputTime = Time.time;
        comboCounter++;

        if (comboCounter > 3)
        {
            comboCounter = 1;
        }

        animator.SetInteger("AttackCombo", comboCounter);
    }

    private void ResetCombo()
    {
        if (comboCounter == 0) return; // Zaten sýfýrsa tekrar iþlem yapma.
        comboCounter = 0;
        animator.SetInteger("AttackCombo", 0);
    }

    public void AnimationEvent_DealDamage()
    {
        if (currentTarget != null && currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            if (comboCounter == 3) // Son vuruþ bonusu
            {
                damage = Mathf.RoundToInt(damage * 1.5f);
                Debug.Log("KOMBO FÝNAL VURUÞU: " + damage + " hasar!");
            }
            targetStats.TakeDamage(damage, myStats);
        }
    }

    public void AnimationEvent_ResetCombo()
    {
        ResetCombo();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        animator.SetBool("Speed", moveDirection.magnitude > 0.1f);
        if (moveDirection != Vector3.zero) { transform.rotation = Quaternion.LookRotation(moveDirection); }
    }

    private void AttemptToUseSkill(int skillIndex)
    {
        if (playerTargeting.currentTarget != null && playerTargeting.currentTarget.TryGetComponent(out MonsterController monster))
        {
            ResetCombo();
            transform.LookAt(monster.transform);
            skillHolder.UseSkill(skillIndex, monster);
        }
    }

    private void InitializeUserInterfaces()
    {
        PlayerHUD_Controller.Instance?.InitializeHUD(myStats);
        CharacterStatsUI_Controller.Instance?.Initialize(myStats);
        SkillBar_Controller.Instance?.Initialize(skillHolder);
        QuestTracker_Controller.Instance?.Initialize(GetComponent<QuestLog>());
    }

    private void AddTestContent()
    {
        skillHolder.LearnSkill(Resources.Load<SkillData>("Data/Skills/Güçlü Vuruþ"));
        // Görevler artýk NPC'den alýndýðý için bu satýr kapalý kalmalý.
        // GetComponent<QuestLog>().AddQuest(Resources.Load<QuestData>("Data/Quests/Kurt Avý"));
    }
}