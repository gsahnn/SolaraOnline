// PlayerHUD_Controller.cs (SINGLETON EKLENDÝ)
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
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }
    }

    public void InitializeHUD(CharacterStats stats)
    {
        if (playerStats != null) playerStats.OnStatsChanged -= UpdateUI;
        playerStats = stats;
        playerStats.OnStatsChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (playerStats != null) playerStats.OnStatsChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (playerStats == null) return;
        levelText.text = "Lv. " + playerStats.level;
        healthSlider.maxValue = playerStats.maxHealth;
        healthSlider.value = playerStats.currentHealth;
        manaSlider.maxValue = playerStats.maxMana;
        manaSlider.value = playerStats.currentMana;
        expSlider.maxValue = playerStats.expToNextLevel;
        expSlider.value = playerStats.currentExp;
    }
}