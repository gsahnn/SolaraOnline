// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Aray�z Referanslar�")]
    [SerializeField] private GameObject statsPanel; // Inspector'dan CharacterStats_Panel'i buraya s�r�kle

    // Bile�en Referanslar�
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;

    // Sava� Durum De�i�keni
    private MonsterController currentTarget;

    private void Start()
    {
        // Gerekli bile�enlere referanslar� oyun ba��nda bir kere al�yoruz.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        mainCamera = Camera.main;

        // --- ARAY�Z BA�LANTILARI ---
        // Sahnede var olan UI kontrolc�lerini bulup, onlara bu karakterin statlar�n� dinlemelerini s�yl�yoruz.

        // 1. Oyuncu HUD'�n� (Can/Mana Barlar�) bul ve initialize et.
        PlayerHUD_Controller hud = FindFirstObjectByType<PlayerHUD_Controller>();
        if (hud != null)
        {
            hud.InitializeHUD(myStats);
        }
        else
        {
            Debug.LogError("Sahnede PlayerHUD_Controller bulunamad�! Manager_Scene'i kontrol et.");
        }

        // 2. Karakter Stat� Panelini bul ve initialize et.
        CharacterStatsUI_Controller statsUI = FindFirstObjectByType<CharacterStatsUI_Controller>();
        if (statsUI != null)
        {
            statsUI.Initialize(myStats);
        }
        else
        {
            Debug.LogError("Sahnede CharacterStatsUI_Controller bulunamad�! Manager_Scene'i kontrol et.");
        }

        // Ba�lang��ta stat panelinin kapal� oldu�undan emin ol.
        if (statsPanel != null)
        {
            statsPanel.SetActive(false);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleAttackInput();
        HandleUIInput(); // UI i�in olan giri�leri ayr� bir fonksiyonda toplamak daha temizdir.
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
        // Envanter veya stat paneli gibi bir UI a��kken sald�rmay� engellemek iyi bir fikirdir.
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

    // UI ile ilgili klavye giri�lerini bu fonksiyonda topluyoruz.
    private void HandleUIInput()
    {
        // 'C' tu�una bas�ld���nda Stat Panelini a�/kapat.
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