// Interactable.cs
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrompt; // NPC'nin baþýndaki [E] Konuþ yazýsý
    public UnityEvent OnInteract; // Bu objeyle etkileþime girildiðinde ne olacaðýný Inspector'dan belirleyeceðiz.

    private void Start()
    {
        interactionPrompt.SetActive(false); // Baþlangýçta gizle
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