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

        // YENÝ VE KRÝTÝK BÖLÜM:
        // Her bir görevin kendi OnQuestUpdated event'ine abone oluyoruz.
        newQuestStatus.OnQuestUpdated += RelayQuestUpdate;

        activeQuests.Add(newQuestStatus);

        // Görev eklendiðinde genel event'i tetikle ki UI'da görünsün.
        OnQuestLogUpdated?.Invoke();
    }

    // Bu fonksiyon, herhangi bir görevden "ben güncellendim" sinyali geldiðinde çalýþýr.
    private void RelayQuestUpdate()
    {
        // Gelen bilgiyi doðrudan genel event'e aktararak UI'a haber veriyoruz.
        OnQuestLogUpdated?.Invoke();
    }

    public void AddQuestProgress(string targetName, int amount)
    {
        // Artýk burada 'isCompleted' kontrolü yapmýyoruz! Bu sorumluluk QuestStatus'un.
        foreach (var quest in activeQuests.Where(q => q.questData.targetName == targetName))
        {
            quest.AddProgress(amount);
        }
    }

    // Obje yok edildiðinde event aboneliklerini temizlemek, hafýza sýzýntýlarýný önler.
    private void OnDestroy()
    {
        foreach (var quest in activeQuests)
        {
            quest.OnQuestUpdated -= RelayQuestUpdate;
        }
    }
    public void ClaimReward(QuestData questData)
    {
        QuestStatus questToComplete = activeQuests.Find(q => q.questData == questData);

        if (questToComplete != null && questToComplete.isCompleted)
        {
            // Oyuncunun statlarýna eriþip ödülleri ver.
            CharacterStats playerStats = GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.AddExperience(questToComplete.questData.experienceReward);
                // Altýn ekleme sistemi henüz yok, þimdilik Debug.Log ile yapalým.
                Debug.Log(questToComplete.questData.goldReward + " altýn kazanýldý!");
            }

            // Tamamlanan görevi aktif listeden kaldýr.
            // Ýleride "Tamamlanmýþ Görevler" listesine ekleyebiliriz.
            activeQuests.Remove(questToComplete);

            // UI'ýn güncellenmesi için event'i tetikle.
            OnQuestLogUpdated?.Invoke();
        }
    }
}  
