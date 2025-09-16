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
        // Girdi kontrol�n� buraya al�yoruz.
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(this.gameObject);
        }

        CheckForInteractable();
    }

    void CheckForInteractable()
    {
        // I��n� kameradan de�il, karakterin kendisinden, bakt��� y�ne do�ru atal�m.
        // Bu, izometrik kamera i�in daha g�venilirdir.
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