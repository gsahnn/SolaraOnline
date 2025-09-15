// DialogueTrigger.cs
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Diyalog Metni")]
    [TextArea(3, 10)] // Metin kutusunu Inspector'da daha b�y�k g�sterir.
    [SerializeField] private string sentenceToSay;

    // Bu fonksiyon PUBLIC olmal� ki UnityEvent onu g�rebilsin.
    // Bu fonksiyonun H��B�R PARAMETRES� YOK.
    public void TriggerDialogue()
    {
        // Singleton �zerinden DialogueSystem'e ula� ve kendi metnini g�nder.
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue(sentenceToSay);
        }
    }
}