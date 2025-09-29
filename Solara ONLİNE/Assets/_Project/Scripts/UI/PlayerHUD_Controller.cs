using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD_Controller : MonoBehaviour
{
    public static PlayerHUD_Controller Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider expSlider;

    private CharacterStats playerStats;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    public void InitializeHUD(CharacterStats stats)
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

        levelText.text = "Lv. " + stats.level;
        healthSlider.maxValue = stats.maxHealth;
        healthSlider.value = stats.currentHealth;
        manaSlider.maxValue = stats.maxMana;
        manaSlider.value = stats.currentMana;
        expSlider.maxValue = stats.expToNextLevel;
        expSlider.value = stats.currentExp;
    }
}