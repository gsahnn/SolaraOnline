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
        // Karakterin tam �n�ndeki objeyi kontrol et
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                // E�er yeni bir obje bulduysak ve bu �nceki de�ilse...
                if (interactable != currentInteractable)
                {
                    // Eski objenin yaz�s�n� gizle (e�er varsa)
                    if (currentInteractable != null) currentInteractable.ShowPrompt(false);

                    // Yeni objeyi ata ve yaz�s�n� g�ster.
                    currentInteractable = interactable;
                    currentInteractable.ShowPrompt(true);
                }
                return; // Bir obje bulduk, devam etmeye gerek yok.
            }
        }

        // E�er ���n hi�bir �eye �arpmad�ysa veya �arpt��� �ey Interactable de�ilse...
        if (currentInteractable != null)
        {
            currentInteractable.ShowPrompt(false);
            currentInteractable = null;
        }
    }
}