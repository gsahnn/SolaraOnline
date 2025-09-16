using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class QuestLog : MonoBehaviour
{
    public List<QuestStatus> activeQuests = new List<QuestStatus>();
    public event Action OnQuestLogUpdated;

    public void AddQuest(QuestData newQuest)
    {
        if (newQuest == null || activeQuests.Any(q => q.questData == newQuest)) return;

        QuestStatus newQuestStatus = new QuestStatus(newQuest);

        // Yeni görevin durum güncellemelerini dinlemeye baþla.
        newQuestStatus.OnQuestStatusUpdated += HandleQuestUpdate;

        activeQuests.Add(newQuestStatus);
        Debug.Log("<color=cyan>QUEST LOG:</color> '" + newQuest.questName + "' görevi eklendi.");

        // Görev eklendiðinde UI'a haber ver.
        OnQuestLogUpdated?.Invoke();
    }

    // Bir QuestStatus güncellendiðinde bu fonksiyon tetiklenir.
    private void HandleQuestUpdate(QuestStatus questStatus)
    {
        // Gelen bilgiyi doðrudan yukarýya, UI'a iletiyoruz.
        OnQuestLogUpdated?.Invoke();
    }

    public void AddQuestProgress(string targetName, int amount)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questData.targetName == targetName)
            {
                quest.AddProgress(amount);
            }
        }
    }
}