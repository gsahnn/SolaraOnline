// PlayerHUD_Controller.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD_Controller : MonoBehaviour
{
    [Header("UI Elemanlar�")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider expSlider;

    // Referanslar
    private CharacterStats playerStats;

    // Bu fonksiyon, GameManager gibi bir y�netici taraf�ndan, oyun d�nyas� y�klendikten
    // ve oyuncu spawn olduktan sonra �a�r�lacak.
    public void InitializeHUD(CharacterStats stats)
    {
        playerStats = stats;

        // Player'�n statlar� her de�i�ti�inde UpdateUI fonksiyonunu �al��t�rmak i�in
        // OnStatsChanged event'ine abone oluyoruz.
        playerStats.OnStatsChanged += UpdateUI;

        // Aray�z� ilk de�erlerle doldur.
        UpdateUI();
    }

    // Bu event'ten ayr�lmay� unutmamak, sahne ge�i�lerinde ve obje yok oldu�unda
    // haf�za s�z�nt�lar�n� (memory leak) �nler.
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

        // Slider'lar�n ve metinlerin de�erlerini oyuncunun mevcut statlar�na g�re ayarla.
        levelText.text = "Lv. " + playerStats.level;

        healthSlider.maxValue = playerStats.maxHealth;
        healthSlider.value = playerStats.currentHealth;

        manaSlider.maxValue = playerStats.maxMana;
        manaSlider.value = playerStats.currentMana;

        expSlider.maxValue = playerStats.expToNextLevel;
        expSlider.value = playerStats.currentExp;
    }
}