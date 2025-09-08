using UnityEngine;
using TMPro;

public class CharacterStatsUI_Controller : MonoBehaviour
{
    public static CharacterStatsUI_Controller Instance { get; private set; }

    [SerializeField] private GameObject characterStatsPanel;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    [SerializeField] private TextMeshProUGUI strText;
    [SerializeField] private TextMeshProUGUI vitText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject strButton;
    [SerializeField] private GameObject vitButton;
    [SerializeField] private GameObject dexButton;
    [SerializeField] private GameObject intButton;

    private CharacterStats playerStats;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }

        characterStatsPanel.SetActive(false);
    }

    public void Initialize(CharacterStats stats)
    {
        if (playerStats != null) playerStats.OnStatsChanged -= UpdateUI;
        playerStats = stats;
        playerStats.OnStatsChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        if (playerStats != null) playerStats.OnStatsChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (playerStats == null) return;

        hpText.text = $"HP: {playerStats.currentHealth} / {playerStats.maxHealth}";
        mpText.text = $"MP: {playerStats.currentMana} / {playerStats.maxMana}";
        strText.text = $"STR: {playerStats.strength}";
        vitText.text = $"VIT: {playerStats.vitality}";
        dexText.text = $"DEX: {playerStats.dexterity}";
        intText.text = $"INT: {playerStats.intelligence}";
        pointsText.text = $"Kalan Puan: {playerStats.statPointsToAssign}";

        bool hasPoints = playerStats.statPointsToAssign > 0;
        strButton.SetActive(hasPoints);
        vitButton.SetActive(hasPoints);
        dexButton.SetActive(hasPoints);
        intButton.SetActive(hasPoints);
    }

    public void TogglePanel()
    {
        characterStatsPanel.SetActive(!characterStatsPanel.activeSelf);
        if (characterStatsPanel.activeSelf) UpdateUI();
    }

    public bool IsOpen()
    {
        return characterStatsPanel.activeSelf;
    }

    public void OnAssignStr() { if (playerStats != null) playerStats.AssignStatPoint("STR"); }
    public void OnAssignVit() { if (playerStats != null) playerStats.AssignStatPoint("VIT"); }
    public void OnAssignDex() { if (playerStats != null) playerStats.AssignStatPoint("DEX"); }
    public void OnAssignInt() { if (playerStats != null) playerStats.AssignStatPoint("INT"); }
}