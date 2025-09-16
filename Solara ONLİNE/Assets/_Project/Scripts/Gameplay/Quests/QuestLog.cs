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

        // YEN� VE KR�T�K B�L�M:
        // Her bir g�revin kendi OnQuestUpdated event'ine abone oluyoruz.
        newQuestStatus.OnQuestUpdated += RelayQuestUpdate;

        activeQuests.Add(newQuestStatus);

        // G�rev eklendi�inde genel event'i tetikle ki UI'da g�r�ns�n.
        OnQuestLogUpdated?.Invoke();
    }

    // Bu fonksiyon, herhangi bir g�revden "ben g�ncellendim" sinyali geldi�inde �al���r.
    private void RelayQuestUpdate()
    {
        // Gelen bilgiyi do�rudan genel event'e aktararak UI'a haber veriyoruz.
        OnQuestLogUpdated?.Invoke();
    }

    public void AddQuestProgress(string targetName, int amount)
    {
        // Art�k burada 'isCompleted' kontrol� yapm�yoruz! Bu sorumluluk QuestStatus'un.
        foreach (var quest in activeQuests.Where(q => q.questData.targetName == targetName))
        {
            quest.AddProgress(amount);
        }
    }

    // Obje yok edildi�inde event aboneliklerini temizlemek, haf�za s�z�nt�lar�n� �nler.
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
            // Oyuncunun statlar�na eri�ip �d�lleri ver.
            CharacterStats playerStats = GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.AddExperience(questToComplete.questData.experienceReward);
                // Alt�n ekleme sistemi hen�z yok, �imdilik Debug.Log ile yapal�m.
                Debug.Log(questToComplete.questData.goldReward + " alt�n kazan�ld�!");
            }

            // Tamamlanan g�revi aktif listeden kald�r.
            // �leride "Tamamlanm�� G�revler" listesine ekleyebiliriz.
            activeQuests.Remove(questToComplete);

            // UI'�n g�ncellenmesi i�in event'i tetikle.
            OnQuestLogUpdated?.Invoke();
        }
    }
}  
