using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button closeButton; // Kapatma butonu referans�

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    private void Start()
    {
        // Butona t�kland���nda CloseDialogue fonksiyonunu �a��rmas� i�in bir listener ekle.
        closeButton.onClick.AddListener(CloseDialogue);
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string sentence) // Sadece tek bir c�mle alacak �ekilde basitle�tirelim.
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = sentence;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}