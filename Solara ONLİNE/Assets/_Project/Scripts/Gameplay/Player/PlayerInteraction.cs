// PlayerInteraction.cs
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    private Interactable currentInteractable;

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractable()
    {
        RaycastHit hit;
        // Karakterin tam önündeki objeyi kontrol et
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                // Eðer yeni bir obje bulduysak ve bu önceki deðilse...
                if (interactable != currentInteractable)
                {
                    // Eski objenin yazýsýný gizle (eðer varsa)
                    if (currentInteractable != null) currentInteractable.ShowPrompt(false);

                    // Yeni objeyi ata ve yazýsýný göster.
                    currentInteractable = interactable;
                    currentInteractable.ShowPrompt(true);
                }
                return; // Bir obje bulduk, devam etmeye gerek yok.
            }
        }

        // Eðer ýþýn hiçbir þeye çarpmadýysa veya çarptýðý þey Interactable deðilse...
        if (currentInteractable != null)
        {
            currentInteractable.ShowPrompt(false);
            currentInteractable = null;
        }
    }
}