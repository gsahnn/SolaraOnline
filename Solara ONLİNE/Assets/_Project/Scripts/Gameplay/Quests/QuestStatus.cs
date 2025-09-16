using System;
using UnityEngine;

[Serializable]
public class QuestStatus
{
    public QuestData questData;
    public int currentAmount;
    public bool isCompleted;

    // Bu event, bu spesifik görevin ilerlemesi veya tamamlanmasý durumunda tetiklenir.
    public event Action<QuestStatus> OnQuestStatusUpdated;

    public QuestStatus(QuestData data)
    {
        questData = data;
        currentAmount = 0;
        isCompleted = false;
    }

    public void AddProgress(int amount)
    {
        // Görev zaten tamamlandýysa, tekrar ilerleme kaydetme.
        if (isCompleted) return;

        currentAmount += amount;

        // Görev hedefine ulaþýldýysa veya geçildiyse...
        if (currentAmount >= questData.requiredAmount)
        {
            currentAmount = questData.requiredAmount; // Sayýyý hedefe sabitle.
            isCompleted = true;
            Debug.Log("<color=green>GÖREV TAMAMLANDI:</color> " + questData.questName);
        }

        // Ýlerleme kaydedildiðinde veya görev tamamlandýðýnda event'i tetikle.
        OnQuestStatusUpdated?.Invoke(this);
    }
}