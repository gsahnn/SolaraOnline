// Interactable.cs
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrompt; // NPC'nin ba��ndaki [E] Konu� yaz�s�
    public UnityEvent OnInteract; // Bu objeyle etkile�ime girildi�inde ne olaca��n� Inspector'dan belirleyece�iz.

    private void Start()
    {
        interactionPrompt.SetActive(false); // Ba�lang��ta gizle
    }

    public void ShowPrompt(bool show)
    {
        interactionPrompt.SetActive(show);
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }
}