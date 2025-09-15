// DialogueSystem.cs
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string[] sentences)
    {
        // �imdilik sadece ilk c�mleyi g�sterelim.
        if (sentences.Length > 0)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = sentences[0];
        }
    }

    // Diyalog penceresini kapatmak i�in (bir butona ba�lanabilir)
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}