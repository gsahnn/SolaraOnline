// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    // Bile�en Referanslar�
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private CharacterStats myStats;

    // Sava� Durum De�i�keni
    private MonsterController currentTarget; // Hedef hala MonsterController tipinde

    private void Start()
    {
        // Bile�en referanslar�n� al.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        mainCamera = Camera.main;

        // HUD kontrolc�s�n� bul ve kendini ona tan�t.
        PlayerHUD_Controller hud = FindFirstObjectByType<PlayerHUD_Controller>();
        if (hud != null)
        {
            hud.InitializeHUD(myStats);
        }
        else
        {
            // E�er HUD bulunamazsa, bu �nemli bir hatad�r. Konsolda belirtelim.
            Debug.LogError("Sahnede PlayerHUD_Controller bulunamad�! Manager_Scene'i kontrol et.");
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleAttackInput();
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

    // Animasyon Event'i taraf�ndan �a�r�lacak olan fonksiyon (D�ZELT�LD�).
    public void AnimationEvent_DealDamage()
    {
        // Hedefimiz hala var m� ve hedefin bir CharacterStats bile�eni var m�?
        if (currentTarget != null && currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            // Rastgele bir hasar de�eri hesapla
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);

            // Hedefin CharacterStats bile�enindeki TakeDamage fonksiyonunu �a��r.
            // Sald�ran�n kim oldu�unu da (bizim statlar�m�z�) parametre olarak g�nder.
            targetStats.TakeDamage(damage, myStats);
        }

        currentTarget = null;
    }
}
