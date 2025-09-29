using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] private Button strButton;
    [SerializeField] private Button vitButton;
    [SerializeField] private Button dexButton;
    [SerializeField] private Button intButton;

    private CharacterStats playerStats;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        if (characterStatsPanel != null) characterStatsPanel.SetActive(false);
    }

    public void Initialize(CharacterStats stats)
    {
        if (playerStats != null) playerStats.OnStatsChanged -= UpdateUI;
        playerStats = stats;
        playerStats.OnStatsChanged += UpdateUI;
        UpdateUI(stats);
    }

    private void OnDestroy()
    {
        if (playerStats != null) playerStats.OnStatsChanged -= UpdateUI;
    }

    // Bu fonksiyon artýk OnStatsChanged event'inin (Action<CharacterStats>) imzasýný karþýlýyor.
    private void UpdateUI(CharacterStats stats)
    {
        if (stats == null) return;

        hpText.text = $"HP: {stats.currentHealth} / {stats.maxHealth}";
        mpText.text = $"MP: {stats.currentMana} / {stats.maxMana}";
        strText.text = $"STR: {stats.strength}";
        vitText.text = $"VIT: {stats.vitality}";
        dexText.text = $"DEX: {stats.dexterity}";
        intText.text = $"INT: {stats.intelligence}";
        pointsText.text = $"Kalan Puan: {stats.statPointsToAssign}";

        bool hasPoints = stats.statPointsToAssign > 0;
        strButton.gameObject.SetActive(hasPoints);
        vitButton.gameObject.SetActive(hasPoints);
        dexButton.gameObject.SetActive(hasPoints);
        intButton.gameObject.SetActive(hasPoints);
    }

    public void OnAssignStr() { if (playerStats != null) playerStats.AssignStatPoint("STR"); }
    public void OnAssignVit() { if (playerStats != null) playerStats.AssignStatPoint("VIT"); }
    public void OnAssignDex() { if (playerStats != null) playerStats.AssignStatPoint("DEX"); }
    public void OnAssignInt() { if (playerStats != null) playerStats.AssignStatPoint("INT"); }

    public void TogglePanel()
    {
        if (characterStatsPanel != null)
        {
            bool isActive = !characterStatsPanel.activeSelf;
            characterStatsPanel.SetActive(isActive);

            if (isActive)
            {
                UpdateUI(playerStats);
            }
        }
    }

    // HATA VEREN EKSÝK FONKSÝYON ÝÇÝ
    public bool IsOpen()
    {
        return characterStatsPanel != null && characterStatsPanel.activeSelf;
    }
}