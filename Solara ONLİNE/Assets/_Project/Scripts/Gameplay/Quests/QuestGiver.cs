// QuestGiver.cs (YEN� VE TAM HAL�)
using UnityEngine;
using System.Linq; // LINQ i�in

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private QuestData questToGive;
    private QuestLog playerQuestLog;

    // Bu fonksiyon Interactable taraf�ndan �a�r�l�r.
    public void OnInteract(GameObject interactor)
    {
        // Etkile�ime giren objenin QuestLog'unu al.
        playerQuestLog = interactor.GetComponent<QuestLog>();
        if (playerQuestLog == null) return; // Etkile�ime giren oyuncu de�ilse bir �ey yapma.

        QuestStatus currentStatus = playerQuestLog.activeQuests.FirstOrDefault(q => q.questData == questToGive);

        if (currentStatus == null)
        {
            // G�rev hi� al�nmam��sa, onay penceresi g�ster.
            DialogueSystem.Instance.ShowConfirmation(questToGive.startDialogue, AcceptQuest);
        }
        else
        {
            if (currentStatus.isCompleted)
            {
                // G�rev tamamlanm��sa, �d�l� ver.
                DialogueSystem.Instance.ShowDialogue(questToGive.completedDialogue);
                playerQuestLog.ClaimReward(questToGive);
            }
            else
            {
                // G�rev devam ediyorsa, bilgilendirme diyalo�u g�ster.
                DialogueSystem.Instance.ShowDialogue(questToGive.inProgressDialogue);
            }
        }
    }

    private void AcceptQuest()
    {
        if (playerQuestLog != null)
        {
            playerQuestLog.AddQuest(questToGive);
        }
    }
}