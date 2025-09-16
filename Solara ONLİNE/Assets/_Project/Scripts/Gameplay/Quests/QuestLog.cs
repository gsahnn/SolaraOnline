// QuestLog.cs
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // LINQ k�t�phanesi

public class QuestLog : MonoBehaviour
{
    public List<QuestStatus> activeQuests = new List<QuestStatus>();

    public void AddQuest(QuestData newQuest)
    {
        if (!activeQuests.Any(q => q.questData == newQuest)) // G�rev zaten al�nmam��sa...
        {
            activeQuests.Add(new QuestStatus(newQuest));
            Debug.Log(newQuest.questName + " g�revi al�nd�.");
        }
    }

    // Bir canavar �ld���nde veya bir e�ya topland���nda bu fonksiyon �a�r�lacak.
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