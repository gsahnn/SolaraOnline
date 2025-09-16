// QuestTracker_Controller.cs
using System.Collections.Generic;
using UnityEngine;

public class QuestTracker_Controller : MonoBehaviour
{
    public static QuestTracker_Controller Instance { get; private set; }

    [SerializeField] private GameObject questTrackerPanel;
    [SerializeField] private GameObject questEntryPrefab; // QuestTrackerEntry_UI prefab'ý
    [SerializeField] private Transform entryContainer;    // QuestTracker_Panel'in kendisi

    private QuestLog playerQuestLog;
    private List<GameObject> spawnedEntries = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    public void Initialize(QuestLog questLog)
    {
        if (playerQuestLog != null) playerQuestLog.OnQuestLogUpdated -= UpdateTrackerUI;

        playerQuestLog = questLog;
        playerQuestLog.OnQuestLogUpdated += UpdateTrackerUI; // Event'e abone ol

        UpdateTrackerUI();
    }

    public void UpdateTrackerUI()
    {
        // Önce eski görev girdilerini temizle
        foreach (GameObject entry in spawnedEntries)
        {
            Destroy(entry);
        }
        spawnedEntries.Clear();

        if (playerQuestLog == null || playerQuestLog.activeQuests.Count == 0)
        {
            questTrackerPanel.SetActive(false);
            return;
        }

        questTrackerPanel.SetActive(true);

        // Aktif her görev için yeni bir UI girdisi oluþtur
        foreach (QuestStatus quest in playerQuestLog.activeQuests)
        {
            GameObject newEntry = Instantiate(questEntryPrefab, entryContainer);
            newEntry.GetComponent<QuestTrackerEntry_Controller>().SetQuest(quest);
            spawnedEntries.Add(newEntry);
        }
    }
}