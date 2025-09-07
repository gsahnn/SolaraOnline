// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))] // Animator bileþenini de zorunlu hale getirelim.
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Savaþ Ayarlarý")]
    [SerializeField] private int attackDamage = 50;
    //[SerializeField] private GameObject hitEffectPrefab; // Ýleride vuruþ efekti eklemek için (þimdilik kapalý)
    //[SerializeField] private AudioClip swordSwingSound; // Kýlýç savurma sesi için (þimdilik kapalý)
    //[SerializeField] private AudioClip hitSound;      // Vuruþ sesi için (þimdilik kapalý)

    // Bileþen Referanslarý
    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;

    // Savaþ Durum Deðiþkeni
    private MonsterController currentTarget; // O anki saldýrý hedefimiz

    private void Start()
    {
        // Gerekli bileþenlere referanslarý oyun baþýnda bir kere alýyoruz. Bu performans için önemlidir.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Her frame'de hareketi ve saldýrý giriþini kontrol et.
        HandleMovement();
        HandleAttackInput();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized; // normalized, çapraz giderken hýzlanmayý önler.

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Karakterin yürüdüðünü veya durduðunu animatöre bildiriyoruz.
        // Hýzýn büyüklüðü (magnitude) 0'dan büyükse karakter hareket ediyor demektir.
        animator.SetBool("IsMoving", moveDirection.magnitude > 0.1f);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void HandleAttackInput()
    {
        // Fareye týklandýysa VE karakter zaten saldýrmýyorsa...
        // "isAttacking" kontrolü, animasyon bitmeden tekrar tekrar saldýrmayý engeller.
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Bir canavara mý týkladýk?
                if (hit.collider.TryGetComponent(out MonsterController monster))
                {
                    // Saldýrýyý baþlat
                    StartAttack(monster);
                }
            }
        }
    }

    private void StartAttack(MonsterController target)
    {
        // Hedefi ve hedefin yönünü sakla
        currentTarget = target;
        transform.LookAt(target.transform.position);

        // Animatör'deki "Attack" trigger'ýný ateþle.
        animator.SetTrigger("Attack");

        // Kýlýç savurma sesini burada oynatabiliriz (ileride eklenecek).
        // AudioSource.PlayOneShot(swordSwingSound);
    }

    // !!! ÖNEMLÝ !!!
    // BU FONKSÝYON, KOD ÝÇÝNDEN DEÐÝL, DOÐRUDAN ANÝMASYONUN KENDÝSÝ TARAFINDAN ÇAÐRILACAKTIR.
    // Bu yüzden public olmalý.
    public void AnimationEvent_DealDamage()
    {
        // Eðer bir hedefimiz varsa (saldýrý iptal edilmediyse)...
        if (currentTarget != null)
        {
            // Vuruþ efektini ve sesini burada oluþtur/oynat.
            // Instantiate(hitEffectPrefab, currentTarget.transform.position, Quaternion.identity);
            // AudioSource.PlayOneShot(hitSound);

            // Hedefe hasar ver.
            currentTarget.TakeDamage(attackDamage);

            // Bu vuruþ için hedefle iþimiz bitti, bir sonraki saldýrýya kadar temizleyelim.
            currentTarget = null;
        }
    }
}