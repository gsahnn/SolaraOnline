using System;
using UnityEngine;

[Serializable]
public class QuestStatus
{
    public QuestData questData;
    public int currentAmount;
    public bool isCompleted;

    // Bu event, bu görevin durumu her deðiþtiðinde tetiklenir.
    public event Action OnQuestUpdated;

    public QuestStatus(QuestData data)
    {
        questData = data;
        currentAmount = 0;
        isCompleted = false;
    }

    public void AddProgress(int amount)
    {
        // Görev zaten tamamlanmýþsa, hiçbir þey yapma.
        if (isCompleted) return;

        currentAmount += amount;

        bool justCompleted = false;
        if (currentAmount >= questData.requiredAmount)
        {
            currentAmount = questData.requiredAmount;
            isCompleted = true;
            justCompleted = true;
        }

        // Ýlerleme kaydedildiðinde event'i tetikle.
        OnQuestUpdated?.Invoke();

        if (justCompleted)
        {
            Debug.Log("<color=green>GÖREV TAMAMLANDI:</color> " + questData.questName);
        }
    }
}