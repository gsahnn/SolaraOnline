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

    [Header("Sava� Ayarlar�")]
    [SerializeField] private LayerMask attackableLayers; // Sald�r�labilir katmanlar� Inspector'dan se�ece�iz

    // ... (di�er de�i�kenler ayn�) ...
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;
    private SkillHolder skillHolder;
    private MonsterController currentTarget;

    // ... Awake() ve Start() fonksiyonlar� ayn� ...
    #region Startup
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
        if (mainCamera == null) Debug.LogError("Sahnede 'MainCamera' etiketli bir kamera bulunamad�!");
        AddTestSkills();
        InitializeUserInterfaces();
    }
    #endregion

    private void Update()
    {
        if (CharacterStatsUI_Controller.Instance != null && CharacterStatsUI_Controller.Instance.IsOpen()) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            HandleMovement();
            HandleInput();
        }
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
        // Raycast'e hangi katmanlar� tarayaca��n� s�yl�yoruz.
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, attackableLayers))
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
        // Raycast'e hangi katmanlar� tarayaca��n� s�yl�yoruz.
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, attackableLayers))
        {
            if (hit.collider.TryGetComponent(out MonsterController monster))
            {
                transform.LookAt(monster.transform);
                skillHolder.UseSkill(skillIndex, monster);
            }
        }
    }

    // De�i�meyen di�er t�m kodlar ayn�
    #region Unchanged Methods
    private void InitializeUserInterfaces() { /*...*/ }
    private void AddTestSkills() { /*...*/ }
    private void HandleMovement() { /*...*/ }
    private void StartAttack(MonsterController target) { /*...*/ }
    public void AnimationEvent_DealDamage() { /*...*/ }
    #endregion
}