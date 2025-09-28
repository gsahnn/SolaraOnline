using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(SkillHolder))]
[RequireComponent(typeof(QuestLog))]
[RequireComponent(typeof(PlayerTargeting))] // Bu script'in varl���n� zorunlu k�l
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Sava� Ayarlar�")]
    [SerializeField] private LayerMask attackableLayers;
    [SerializeField] private float comboResetTime = 1.5f;

    // Sistem Referanslar�
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;
    private SkillHolder skillHolder;
    private PlayerTargeting playerTargeting; // <-- EKS�K DE���KEN TANIMI BURADAYDI
    private MonsterController currentTarget;

    // Kombo Sistemi De�i�kenleri
    private int comboCounter = 0;
    private float lastAttackInputTime = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        skillHolder = GetComponent<SkillHolder>();
        playerTargeting = GetComponent<PlayerTargeting>(); // <-- EKS�K ATAMA BURADAYDI
    }

    private void Start()
    {
        mainCamera = Camera.main;
        AddTestContent();
        InitializeUserInterfaces();
    }

    private void Update()
    {
        if (Time.time - lastAttackInputTime > comboResetTime)
        {
            ResetCombo();
        }

        bool isUIOpen = CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen();
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        if (isUIOpen) return;

        if (!isAttacking)
        {
            HandleMovement();
        }

        HandleInput();
    }

    private void HandleInput()
    {
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
        comboCounter = 0;
        animator.SetInteger("AttackCombo", 0);
    }

    public void AnimationEvent_DealDamage()
    {
        if (currentTarget != null && currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            if (comboCounter == 3)
            {
                damage = Mathf.RoundToInt(damage * 1.5f);
                Debug.Log("KOMBO F�NAL VURU�U: " + damage + " hasar!");
            }
            targetStats.TakeDamage(damage, myStats);
        }
    }

    public void AnimationEvent_ResetCombo()
    {
        ResetCombo();
    }

    private void AttemptToUseSkill(int skillIndex)
    {
        // Hatal� `monster` kullan�m� d�zeltildi.
        if (playerTargeting.currentTarget != null && playerTargeting.currentTarget.TryGetComponent(out MonsterController targetMonster))
        {
            ResetCombo();
            transform.LookAt(targetMonster.transform); // Y�nelme eklendi
            skillHolder.UseSkill(skillIndex, targetMonster);
        }
    }

    // --- Di�er Fonksiyonlar (De�i�iklik Yok) ---
    #region Unchanged Helper Functions
    private void AddTestContent()
    {
        skillHolder.LearnSkill(Resources.Load<SkillData>("Data/Skills/G��l� Vuru�"));
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
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // H�z� normalize et ki �apraz giderken h�zlanmas�n.
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // --- EN �NEML� SATIR BURASI ---
        // Karakterin mevcut h�z�n� hesapla ve bunu Animator'deki 'Speed' parametresine yaz.
        // 0.1f'lik 'damp time' ge�i�i yumu�at�r.
        float currentSpeed = moveDirection.magnitude;
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);
        // --------------------------------

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    #endregion
}
}