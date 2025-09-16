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
        if (newQuest == null)
        {
            Debug.LogError("Bo� (null) bir g�rev eklenmeye �al���ld�.");
            return;
        }

        if (!activeQuests.Any(q => q.questData == newQuest))
        {
            activeQuests.Add(new QuestStatus(newQuest));
            Debug.Log("<color=cyan>QUEST LOG:</color> '" + newQuest.questName + "' g�revi QuestLog'a eklendi.");
            OnQuestLogUpdated?.Invoke();
        }
    }

    public void AddQuestProgress(string targetName, int amount)
    {
        bool questUpdated = false;
        foreach (var quest in activeQuests)
        {
            if (!quest.isCompleted && quest.questData.targetName == targetName)
            {
                quest.AddProgress(amount);
                questUpdated = true;
            }
        }
        if (questUpdated) OnQuestLogUpdated?.Invoke();
    }
}