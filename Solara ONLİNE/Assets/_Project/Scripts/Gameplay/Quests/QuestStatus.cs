// QuestStatus.cs
using UnityEngine;
[System.Serializable]
public class QuestStatus
{
    public QuestData questData;
    public int currentAmount;
    public bool isCompleted;

    public QuestStatus(QuestData data)
    {
        questData = data;
        currentAmount = 0;
        isCompleted = false;
    }

    public void AddProgress(int amount)
    {
        currentAmount += amount;
        if (currentAmount >= questData.requiredAmount)
        {
            isCompleted = true;
            Debug.Log(questData.questName + " görevi tamamlandý!");
        }
    }
}