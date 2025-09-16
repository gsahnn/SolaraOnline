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

        // Yeni g�revin durum g�ncellemelerini dinlemeye ba�la.
        newQuestStatus.OnQuestStatusUpdated += HandleQuestUpdate;

        activeQuests.Add(newQuestStatus);
        Debug.Log("<color=cyan>QUEST LOG:</color> '" + newQuest.questName + "' g�revi eklendi.");

        // G�rev eklendi�inde UI'a haber ver.
        OnQuestLogUpdated?.Invoke();
    }

    // Bir QuestStatus g�ncellendi�inde bu fonksiyon tetiklenir.
    private void HandleQuestUpdate(QuestStatus questStatus)
    {
        // Gelen bilgiyi do�rudan yukar�ya, UI'a iletiyoruz.
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