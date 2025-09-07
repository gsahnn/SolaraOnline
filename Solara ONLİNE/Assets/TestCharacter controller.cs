// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))] // Animator bile�enini de zorunlu hale getirelim.
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Sava� Ayarlar�")]
    [SerializeField] private int attackDamage = 50;
    //[SerializeField] private GameObject hitEffectPrefab; // �leride vuru� efekti eklemek i�in (�imdilik kapal�)
    //[SerializeField] private AudioClip swordSwingSound; // K�l�� savurma sesi i�in (�imdilik kapal�)
    //[SerializeField] private AudioClip hitSound;      // Vuru� sesi i�in (�imdilik kapal�)

    // Bile�en Referanslar�
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;

    // Sava� Durum De�i�keni
    private MonsterController currentTarget; // O anki sald�r� hedefimiz

    private void Start()
    {
        // Gerekli bile�enlere referanslar� oyun ba��nda bir kere al�yoruz. Bu performans i�in �nemlidir.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Her frame'de hareketi ve sald�r� giri�ini kontrol et.
        HandleMovement();
        HandleAttackInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized; // normalized, �apraz giderken h�zlanmay� �nler.

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Karakterin y�r�d���n� veya durdu�unu animat�re bildiriyoruz.
        // H�z�n b�y�kl��� (magnitude) 0'dan b�y�kse karakter hareket ediyor demektir.
        animator.SetBool("IsMoving", moveDirection.magnitude > 0.1f);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void HandleAttackInput()
    {
        // Fareye t�kland�ysa VE karakter zaten sald�rm�yorsa...
        // "isAttacking" kontrol�, animasyon bitmeden tekrar tekrar sald�rmay� engeller.
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Bir canavara m� t�klad�k?
                if (hit.collider.TryGetComponent(out MonsterController monster))
                {
                    // Sald�r�y� ba�lat
                    StartAttack(monster);
                }
            }
        }
    }

    private void StartAttack(MonsterController target)
    {
        // Hedefi ve hedefin y�n�n� sakla
        currentTarget = target;
        transform.LookAt(target.transform.position);

        // Animat�r'deki "Attack" trigger'�n� ate�le.
        animator.SetTrigger("Attack");

        // K�l�� savurma sesini burada oynatabiliriz (ileride eklenecek).
        // AudioSource.PlayOneShot(swordSwingSound);
    }

    // !!! �NEML� !!!
    // BU FONKS�YON, KOD ���NDEN DE��L, DO�RUDAN AN�MASYONUN KEND�S� TARAFINDAN �A�RILACAKTIR.
    // Bu y�zden public olmal�.
    public void AnimationEvent_DealDamage()
    {
        // E�er bir hedefimiz varsa (sald�r� iptal edilmediyse)...
        if (currentTarget != null)
        {
            // Vuru� efektini ve sesini burada olu�tur/oynat.
            // Instantiate(hitEffectPrefab, currentTarget.transform.position, Quaternion.identity);
            // AudioSource.PlayOneShot(hitSound);

            // Hedefe hasar ver.
            currentTarget.TakeDamage(attackDamage);

            // Bu vuru� i�in hedefle i�imiz bitti, bir sonraki sald�r�ya kadar temizleyelim.
            currentTarget = null;
        }
    }
}