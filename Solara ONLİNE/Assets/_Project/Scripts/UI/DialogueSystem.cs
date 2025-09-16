// DialogueSystem.cs (GÜNCELLENMÝÞ HALÝ)
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System; // Action için

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [Header("Diyalog Paneli")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button closeButton;

    [Header("Onay Paneli")] // Görev kabul/reddet için
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;

    private Action onAcceptAction; // Kabul butonuna basýldýðýnda tetiklenecek eylem

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    private void Start()
    {
        closeButton.onClick.AddListener(CloseAllPanels);
        acceptButton.onClick.AddListener(OnAccept);
        declineButton.onClick.AddListener(CloseAllPanels);

        CloseAllPanels();
    }

    // Basit bir mesaj göstermek için
    public void ShowDialogue(string sentence)
    {
        CloseAllPanels();
        dialoguePanel.SetActive(true);
        dialogueText.text = sentence;
    }

    // Bir soru sormak ve onay beklemek için
    public void ShowConfirmation(string question, Action onAccept)
    {
        CloseAllPanels();
        confirmationPanel.SetActive(true);
        confirmationText.text = question;
        onAcceptAction = onAccept;
    }

    private void OnAccept()
    {
        onAcceptAction?.Invoke();
        CloseAllPanels();
    }

    public void CloseAllPanels()
    {
        dialoguePanel.SetActive(false);
        confirmationPanel.SetActive(false);
    }
}