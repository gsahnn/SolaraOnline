using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private Interactable currentInteractable;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Girdi kontrolünü buraya alýyoruz.
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(this.gameObject);
        }

        CheckForInteractable();
    }

    void CheckForInteractable()
    {
        // Iþýný kameradan deðil, karakterin kendisinden, baktýðý yöne doðru atalým.
        // Bu, izometrik kamera için daha güvenilirdir.
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                if (interactable != currentInteractable)
                {
                    if (currentInteractable != null) currentInteractable.HidePrompt();
                    currentInteractable = interactable;
                    currentInteractable.ShowPrompt();
                }
                return;
            }
        }

        if (currentInteractable != null)
        {
            currentInteractable.HidePrompt();
            currentInteractable = null;
        }
    }
}