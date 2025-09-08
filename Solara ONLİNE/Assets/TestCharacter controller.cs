// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Arayüz Referanslarý")]
    [SerializeField] private GameObject statsPanel; // Inspector'dan CharacterStats_Panel'i buraya sürükle

    // Bileþen Referanslarý
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;

    // Savaþ Durum Deðiþkeni
    private MonsterController currentTarget;

    private void Start()
    {
        // Gerekli bileþenlere referanslarý oyun baþýnda bir kere alýyoruz.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        mainCamera = Camera.main;

        // --- ARAYÜZ BAÐLANTILARI ---
        // Sahnede var olan UI kontrolcülerini bulup, onlara bu karakterin statlarýný dinlemelerini söylüyoruz.

        // 1. Oyuncu HUD'ýný (Can/Mana Barlarý) bul ve initialize et.
        PlayerHUD_Controller hud = FindFirstObjectByType<PlayerHUD_Controller>();
        if (hud != null)
        {
            hud.InitializeHUD(myStats);
        }
        else
        {
            Debug.LogError("Sahnede PlayerHUD_Controller bulunamadý! Manager_Scene'i kontrol et.");
        }

        // 2. Karakter Statü Panelini bul ve initialize et.
        CharacterStatsUI_Controller statsUI = FindFirstObjectByType<CharacterStatsUI_Controller>();
        if (statsUI != null)
        {
            statsUI.Initialize(myStats);
        }
        else
        {
            Debug.LogError("Sahnede CharacterStatsUI_Controller bulunamadý! Manager_Scene'i kontrol et.");
        }

        // Baþlangýçta stat panelinin kapalý olduðundan emin ol.
        if (statsPanel != null)
        {
            statsPanel.SetActive(false);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleAttackInput();
        HandleUIInput(); // UI için olan giriþleri ayrý bir fonksiyonda toplamak daha temizdir.
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

    private void HandleAttackInput()
    {
        // Envanter veya stat paneli gibi bir UI açýkken saldýrmayý engellemek iyi bir fikirdir.
        if (statsPanel != null && statsPanel.activeSelf) return;

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

    // UI ile ilgili klavye giriþlerini bu fonksiyonda topluyoruz.
    private void HandleUIInput()
    {
        // 'C' tuþuna basýldýðýnda Stat Panelini aç/kapat.
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (statsPanel != null)
            {
                statsPanel.SetActive(!statsPanel.activeSelf);
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