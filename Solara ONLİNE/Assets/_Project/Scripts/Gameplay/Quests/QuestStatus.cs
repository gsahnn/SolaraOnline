using System;
using UnityEngine;

[Serializable]
public class QuestStatus
{
    public QuestData questData;
    public int currentAmount;
    public bool isCompleted;

    // Bu event, bu spesifik g�revin ilerlemesi veya tamamlanmas� durumunda tetiklenir.
    public event Action<QuestStatus> OnQuestStatusUpdated;

    public QuestStatus(QuestData data)
    {
        questData = data;
        currentAmount = 0;
        isCompleted = false;
    }

    public void AddProgress(int amount)
    {
        // G�rev zaten tamamland�ysa, tekrar ilerleme kaydetme.
        if (isCompleted) return;

        currentAmount += amount;

        // G�rev hedefine ula��ld�ysa veya ge�ildiyse...
        if (currentAmount >= questData.requiredAmount)
        {
            currentAmount = questData.requiredAmount; // Say�y� hedefe sabitle.
            isCompleted = true;
            Debug.Log("<color=green>G�REV TAMAMLANDI:</color> " + questData.questName);
        }

        // �lerleme kaydedildi�inde veya g�rev tamamland���nda event'i tetikle.
        OnQuestStatusUpdated?.Invoke(this);
    }
}