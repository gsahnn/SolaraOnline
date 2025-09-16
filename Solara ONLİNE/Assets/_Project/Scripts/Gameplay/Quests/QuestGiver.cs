// QuestGiver.cs (YENÝ VE TAM HALÝ)
using UnityEngine;
using System.Linq; // LINQ için

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private QuestData questToGive;
    private QuestLog playerQuestLog;

    // Bu fonksiyon Interactable tarafýndan çaðrýlýr.
    public void OnInteract(GameObject interactor)
    {
        // Etkileþime giren objenin QuestLog'unu al.
        playerQuestLog = interactor.GetComponent<QuestLog>();
        if (playerQuestLog == null) return; // Etkileþime giren oyuncu deðilse bir þey yapma.

        QuestStatus currentStatus = playerQuestLog.activeQuests.FirstOrDefault(q => q.questData == questToGive);

        if (currentStatus == null)
        {
            // Görev hiç alýnmamýþsa, onay penceresi göster.
            DialogueSystem.Instance.ShowConfirmation(questToGive.startDialogue, AcceptQuest);
        }
        else
        {
            if (currentStatus.isCompleted)
            {
                // Görev tamamlanmýþsa, ödülü ver.
                DialogueSystem.Instance.ShowDialogue(questToGive.completedDialogue);
                playerQuestLog.ClaimReward(questToGive);
            }
            else
            {
                // Görev devam ediyorsa, bilgilendirme diyaloðu göster.
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