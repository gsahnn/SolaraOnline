// PlayerController.cs
using UnityEngine;

// Karakterimizin hareket edebilmesi için bir CharacterController bileþenine ihtiyacý var.
// Bu satýr, script'i eklediðimiz objeye otomatik olarak CharacterController ekler.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Savaþ Ayarlarý")]
    [SerializeField] private int attackDamage = 50;

    // Referanslar
    private CharacterController controller;
    private Camera mainCamera;

    private void Start()
    {
        // Gerekli bileþenlere referanslarý alýyoruz.
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main; // Sahnedeki ana kamerayý bulur.
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        // Basit klavye giriþi (eski input sistemiyle)
        float horizontal = Input.GetAxis("Horizontal"); // A ve D tuþlarý
        float vertical = Input.GetAxis("Vertical");   // W ve S tuþlarý

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // Karakterin hýzýyla ve her bilgisayarda ayný hýzda çalýþmasý için Time.deltaTime ile çarpýyoruz.
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Karakterin gittiði yöne doðru bakmasýný saðla
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void HandleAttack()
    {
        // Fareye sol týkladýðýmýzda...
        if (Input.GetMouseButtonDown(0))
        {
            // Ekranda týkladýðýmýz yere bir ýþýn gönder.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Eðer bu ýþýn bir MonsterController'a çarparsa...
                if (hit.collider.TryGetComponent(out MonsterController monster))
                {
                    // O canavara hasar ver.
                    monster.TakeDamage(attackDamage);
                }
            }
        }
    }
}