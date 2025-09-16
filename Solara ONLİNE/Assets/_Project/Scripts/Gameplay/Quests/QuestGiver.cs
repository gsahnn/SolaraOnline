using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Interactable))]
public class QuestGiver : MonoBehaviour
{
    [SerializeField] private QuestData questToGive;
    private QuestLog playerQuestLog;

    public void OnInteract(GameObject interactor)
    {
        // 1. Aktif bileþenleri ara.
        playerQuestLog = interactor.GetComponent<QuestLog>();

        // 2. Eðer bulamadýysak, pasif olanlar da dahil olmak üzere tekrar ara.
        if (playerQuestLog == null)
        {
            playerQuestLog = interactor.GetComponentInChildren<QuestLog>(true); // 'true' pasif olanlarý da arar.

            if (playerQuestLog != null)
            {
                // EÐER BURAYA GÝRERSE, SORUN KESÝNLÝKLE BUDUR!
                Debug.LogError("<size=16><color=red>KRÝTÝK HATA:</color> 'QuestLog' script'i Oyuncu prefab'ýnda bulundu ama PASÝF (DISABLED) durumda! Lütfen Player prefab'ýný kontrol edip QuestLog script'inin yanýndaki kutucuðu iþaretleyin.</size>");
                return;
            }
            else
            {
                // Eðer burada bile bulamadýysa, script gerçekten de ekli deðildir.
                Debug.LogError("<size=16><color=red>KRÝTÝK HATA:</color> Oyuncu prefab'ýnda 'QuestLog' script'i HÝÇBÝR ÞEKÝLDE bulunamadý!</size>");
                return;
            }
        }

        // --- Eðer QuestLog bulunduysa, normal akýþ devam eder ---
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