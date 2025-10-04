using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerTargeting), typeof(PlayerCombatController))]
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private PlayerTargeting playerTargeting;

    // CombatController referansına artık ihtiyacımız yok, çünkü girdiyi doğrudan Animator'e veriyoruz.

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
    }

    private void Update()
    {
        if (IsAnyUIPanelOpen())
        {
            // Paneller açıksa tüm hareket ve saldırı girdilerini sıfırla.
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsAttacking", false);
            return;
        }

        HandleMovementInput();
        HandleCombatInput();
        HandleTargetingInput(); // Hedefleme hala kodla yönetiliyor.
        HandleRotation(); // Rotasyon da kodla yönetilmeli.
    }

    private void HandleMovementInput()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        // Hız bilgisini Animator'e gönder. Hareketi Root Motion kendisi yapacak.
        animator.SetFloat("Speed", direction.magnitude, 0.1f, Time.deltaTime);
    }

    private void HandleCombatInput()
    {
        // Saldırı isteğini doğrudan ve sadece Animator'e iletiyoruz.
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("IsAttacking", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    private void HandleRotation()
    {
        // Hedef varsa, anında hedefe dön. Root Motion dönüşü etkilemez.
        if (playerTargeting.currentTarget != null)
        {
            Vector3 targetDir = playerTargeting.currentTarget.transform.position - transform.position;
            targetDir.y = 0;
            if (targetDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(targetDir);
            }
        }
        // Eğer hedef yoksa ve oyuncu hareket ediyorsa, gittiği yöne yavaşça dön.
        // Bu, Root Motion ile birlikte çalışır çünkü sadece rotasyonu etkiler.
        else
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
            }
        }
    }

    private void HandleTargetingInput() { playerTargeting.HandleTargetingInput(); }
    private bool IsAnyUIPanelOpen() { return false; }
}