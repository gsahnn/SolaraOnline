using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Interactable))]
public class QuestGiver : MonoBehaviour
{
    [SerializeField] private QuestData questToGive;
    private QuestLog playerQuestLog;

    public void OnInteract(GameObject interactor)
    {
        // 1. Aktif bile�enleri ara.
        playerQuestLog = interactor.GetComponent<QuestLog>();

        // 2. E�er bulamad�ysak, pasif olanlar da dahil olmak �zere tekrar ara.
        if (playerQuestLog == null)
        {
            playerQuestLog = interactor.GetComponentInChildren<QuestLog>(true); // 'true' pasif olanlar� da arar.

            if (playerQuestLog != null)
            {
                // E�ER BURAYA G�RERSE, SORUN KES�NL�KLE BUDUR!
                Debug.LogError("<size=16><color=red>KR�T�K HATA:</color> 'QuestLog' script'i Oyuncu prefab'�nda bulundu ama PAS�F (DISABLED) durumda! L�tfen Player prefab'�n� kontrol edip QuestLog script'inin yan�ndaki kutucu�u i�aretleyin.</size>");
                return;
            }
            else
            {
                // E�er burada bile bulamad�ysa, script ger�ekten de ekli de�ildir.
                Debug.LogError("<size=16><color=red>KR�T�K HATA:</color> Oyuncu prefab'�nda 'QuestLog' script'i H��B�R �EK�LDE bulunamad�!</size>");
                return;
            }
        }

        // --- E�er QuestLog bulunduysa, normal ak�� devam eder ---
        QuestStatus currentStatus = playerQuestLog.activeQuests.FirstOrDefault(q => q.questData == questToGive);

        if (currentStatus == null)
        {
            DialogueSystem.Instance.ShowConfirmation(questToGive.startDialogue, AcceptQuest);
        }
        else
        {
            if (currentStatus.isCompleted)
            {
                DialogueSystem.Instance.ShowDialogue(questToGive.completedDialogue);
                playerQuestLog.ClaimReward(questToGive);
            }
            else
            {
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