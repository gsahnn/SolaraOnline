// PlayerController.cs
using UnityEngine;

// Karakterimizin hareket edebilmesi i�in bir CharacterController bile�enine ihtiyac� var.
// Bu sat�r, script'i ekledi�imiz objeye otomatik olarak CharacterController ekler.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Sava� Ayarlar�")]
    [SerializeField] private int attackDamage = 50;

    // Referanslar
    private CharacterController controller;
    private Camera mainCamera;

    private void Start()
    {
        // Gerekli bile�enlere referanslar� al�yoruz.
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main; // Sahnedeki ana kameray� bulur.
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        // Basit klavye giri�i (eski input sistemiyle)
        float horizontal = Input.GetAxis("Horizontal"); // A ve D tu�lar�
        float vertical = Input.GetAxis("Vertical");   // W ve S tu�lar�

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // Karakterin h�z�yla ve her bilgisayarda ayn� h�zda �al��mas� i�in Time.deltaTime ile �arp�yoruz.
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Karakterin gitti�i y�ne do�ru bakmas�n� sa�la
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void HandleAttack()
    {
        // Fareye sol t�klad���m�zda...
        if (Input.GetMouseButtonDown(0))
        {
            // Ekranda t�klad���m�z yere bir ���n g�nder.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // E�er bu ���n bir MonsterController'a �arparsa...
                if (hit.collider.TryGetComponent(out MonsterController monster))
                {
                    // O canavara hasar ver.
                    monster.TakeDamage(attackDamage);
                }
            }
        }
    }
}