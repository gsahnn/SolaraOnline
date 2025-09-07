// PlayerHUD_Controller.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD_Controller : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider expSlider;

    // Referanslar
    private CharacterStats playerStats;

    // Bu fonksiyon, GameManager gibi bir yönetici tarafýndan, oyun dünyasý yüklendikten
    // ve oyuncu spawn olduktan sonra çaðrýlacak.
    public void InitializeHUD(CharacterStats stats)
    {
        playerStats = stats;

        // Player'ýn statlarý her deðiþtiðinde UpdateUI fonksiyonunu çalýþtýrmak için
        // OnStatsChanged event'ine abone oluyoruz.
        playerStats.OnStatsChanged += UpdateUI;

        // Arayüzü ilk deðerlerle doldur.
        UpdateUI();
    }

    // Bu event'ten ayrýlmayý unutmamak, sahne geçiþlerinde ve obje yok olduðunda
    // hafýza sýzýntýlarýný (memory leak) önler.
    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnStatsChanged -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        if (playerStats == null) return;

        // Slider'larýn ve metinlerin deðerlerini oyuncunun mevcut statlarýna göre ayarla.
        levelText.text = "Lv. " + playerStats.level;

        healthSlider.maxValue = playerStats.maxHealth;
        healthSlider.value = playerStats.currentHealth;

        manaSlider.maxValue = playerStats.maxMana;
        manaSlider.value = playerStats.currentMana;

        expSlider.maxValue = playerStats.expToNextLevel;
        expSlider.value = playerStats.currentExp;
    }
}