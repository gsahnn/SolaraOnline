// QuestLog.cs
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // LINQ kütüphanesi

public class QuestLog : MonoBehaviour
{
    public List<QuestStatus> activeQuests = new List<QuestStatus>();

    public void AddQuest(QuestData newQuest)
    {
        if (!activeQuests.Any(q => q.questData == newQuest)) // Görev zaten alýnmamýþsa...
        {
            activeQuests.Add(new QuestStatus(newQuest));
            Debug.Log(newQuest.questName + " görevi alýndý.");
        }
    }

    // Bir canavar öldüðünde veya bir eþya toplandýðýnda bu fonksiyon çaðrýlacak.
    public void AddQuestProgress(string targetName, int amount)
    {
        foreach (var quest in activeQuests)
        {
            if (!quest.isCompleted && quest.questData.targetName == targetName)
            {
                quest.AddProgress(amount);
            }
        }
    }
}