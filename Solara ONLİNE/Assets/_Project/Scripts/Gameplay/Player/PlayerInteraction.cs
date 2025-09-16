using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Etkileþim Ayarlarý")]
    [SerializeField] private float interactionDistance = 2.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform interactionRayOrigin; // Iþýnýn nereden baþlayacaðýný belirlemek için

    private Interactable currentInteractable;

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            // Interact fonksiyonuna, etkileþime girenin kim olduðunu (yani bu objeyi) bildir.
            currentInteractable.Interact(this.gameObject);
        }

    void CheckForInteractable()
    {
        // Iþýnýn baþlangýç noktasýný ve yönünü karakterden alýyoruz.
        Vector3 rayOrigin = interactionRayOrigin.position;
        Vector3 rayDirection = transform.forward;

        // Iþýný sahnede görselleþtirelim ki nereye gittiðini görelim.
        Debug.DrawRay(rayOrigin, rayDirection * interactionDistance, Color.cyan);

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, interactionDistance, interactableLayer))
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

        // Iþýn hiçbir þeye çarpmýyorsa veya mevcut hedef menzilden çýktýysa, prompt'u gizle.
        if (currentInteractable != null)
        {
            currentInteractable.HidePrompt();
            currentInteractable = null;
        }
    }
}
}