using System;
using UnityEngine;

[Serializable]
public class QuestStatus
{
    public QuestData questData;
    public int currentAmount;
    public bool isCompleted;

    // Bu event, bu g�revin durumu her de�i�ti�inde tetiklenir.
    public event Action OnQuestUpdated;

    public QuestStatus(QuestData data)
    {
        questData = data;
        currentAmount = 0;
        isCompleted = false;
    }

    public void AddProgress(int amount)
    {
        // G�rev zaten tamamlanm��sa, hi�bir �ey yapma.
        if (isCompleted) return;

        currentAmount += amount;

        bool justCompleted = false;
        if (currentAmount >= questData.requiredAmount)
        {
            currentAmount = questData.requiredAmount;
            isCompleted = true;
            justCompleted = true;
        }

        // �lerleme kaydedildi�inde event'i tetikle.
        OnQuestUpdated?.Invoke();

        if (justCompleted)
        {
            Debug.Log("<color=green>G�REV TAMAMLANDI:</color> " + questData.questName);
        }
    }
}