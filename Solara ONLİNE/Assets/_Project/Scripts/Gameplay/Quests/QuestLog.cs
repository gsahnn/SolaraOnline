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

        activeQuests.Add(new QuestStatus(newQuest));
        OnQuestLogUpdated?.Invoke();
    }

    public void AddQuestProgress(string targetName, int amount)
    {
        bool questUpdated = false;
        foreach (var quest in activeQuests.Where(q => q.questData.targetName == targetName && !q.isCompleted))
        {
            quest.AddProgress(amount);
            questUpdated = true;
        }

        if (questUpdated)
        {
            OnQuestLogUpdated?.Invoke();
        }
    }
}