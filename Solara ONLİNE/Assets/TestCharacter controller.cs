// PlayerController.cs (SON HAL� - ARAMA YOK, SADECE KOMUT VAR)
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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        skillHolder = GetComponent<SkillHolder>();
        mainCamera = Camera.main;

        // Oyuncu sahneye geldi�inde, t�m UI Y�neticilerine kendisini tan�t�r.
        InitializeUserInterfaces();

        // TEST AMA�LI YETENEK EKLEME
        SkillData testSkill = Resources.Load<SkillData>("Data/Skills/G��l� Vuru�");
        if (testSkill != null)
        {
            skillHolder.LearnSkill(testSkill);
        }
        else
        {
            Debug.LogError("'G��l� Vuru�' yetene�i 'Resources/Data/Skills' klas�r�nde bulunamad�!");
        }
    }

    // Oyuncunun varl���n� UI sistemlerine bildiren fonksiyon
    private void InitializeUserInterfaces()
    {
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

    // ... HandleMovement, HandleUIInput, HandleAttackInput, StartAttack, AnimationEvent_DealDamage...
    // ... BU FONKS�YONLARIN HEPS� AYNI KALIYOR ...
    // ... Sadece HandleSkillInput'� g�ncelleyelim ...

    private void HandleSkillInput()
    {
        if (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) { AttemptToUseSkill(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { AttemptToUseSkill(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { AttemptToUseSkill(2); }
    }

    private void AttemptToUseSkill(int skillIndex)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.TryGetComponent(out MonsterController monster))
            {
                transform.LookAt(monster.transform); // Yetenek kullanmadan �nce hedefe d�n.
                skillHolder.UseSkill(skillIndex, monster);
            }
        }
    }

    // De�i�meyen di�er fonksiyonlar
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