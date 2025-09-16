using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Etkile�im Ayarlar�")]
    [SerializeField] private float interactionDistance = 2.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform interactionRayOrigin; // I��n�n nereden ba�layaca��n� belirlemek i�in

    private Interactable currentInteractable;

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            // Interact fonksiyonuna, etkile�ime girenin kim oldu�unu (yani bu objeyi) bildir.
            currentInteractable.Interact(this.gameObject);
        }

    void CheckForInteractable()
    {
        // I��n�n ba�lang�� noktas�n� ve y�n�n� karakterden al�yoruz.
        Vector3 rayOrigin = interactionRayOrigin.position;
        Vector3 rayDirection = transform.forward;

        // I��n� sahnede g�rselle�tirelim ki nereye gitti�ini g�relim.
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

        // I��n hi�bir �eye �arpm�yorsa veya mevcut hedef menzilden ��kt�ysa, prompt'u gizle.
        if (currentInteractable != null)
        {
            currentInteractable.HidePrompt();
            currentInteractable = null;
        }
    }
}
}