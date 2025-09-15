// DialogueTrigger.cs
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Diyalog Metni")]
    [TextArea(3, 10)] // Metin kutusunu Inspector'da daha büyük gösterir.
    [SerializeField] private string sentenceToSay;

    // Bu fonksiyon PUBLIC olmalý ki UnityEvent onu görebilsin.
    // Bu fonksiyonun HÝÇBÝR PARAMETRESÝ YOK.
    public void TriggerDialogue()
    {
        // Singleton üzerinden DialogueSystem'e ulaþ ve kendi metnini gönder.
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue(sentenceToSay);
        }
    }
}