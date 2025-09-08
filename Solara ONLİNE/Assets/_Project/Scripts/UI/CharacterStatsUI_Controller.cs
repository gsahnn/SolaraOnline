// CharacterStatsUI_Controller.cs
using UnityEngine;
using TMPro;

public class CharacterStatsUI_Controller : MonoBehaviour
{
    [Header("UI Metin Alanlarý")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    [SerializeField] private TextMeshProUGUI strText;
    [SerializeField] private TextMeshProUGUI vitText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private TextMeshProUGUI pointsText;

    [Header("Butonlar")]
    [SerializeField] private GameObject strButton;
    [SerializeField] private GameObject vitButton;
    [SerializeField] private GameObject dexButton;
    [SerializeField] private GameObject intButton;

    private CharacterStats playerStats;

    // Bu fonksiyon, PlayerController tarafýndan çaðrýlacak (HUD gibi)
    public void Initialize(CharacterStats stats)
    {
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

        hpText.text = $"HP: {playerStats.currentHealth} / {playerStats.maxHealth}";
        mpText.text = $"MP: {playerStats.currentMana} / {playerStats.maxMana}";
        strText.text = $"STR: {playerStats.strength}";
        vitText.text = $"VIT: {playerStats.vitality}";
        dexText.text = $"DEX: {playerStats.dexterity}";
        intText.text = $"INT: {playerStats.intelligence}";
        pointsText.text = $"Kalan Puan: {playerStats.statPointsToAssign}";

        // Eðer daðýtýlacak puan varsa, butonlarý aktif et. Yoksa pasif yap.
        bool hasPoints = playerStats.statPointsToAssign > 0;
        strButton.SetActive(hasPoints);
        vitButton.SetActive(hasPoints);
        dexButton.SetActive(hasPoints);
        intButton.SetActive(hasPoints);
    }

    // Bu fonksiyonlar, Inspector'dan butonlarýn OnClick() event'lerine atanacak.
    public void OnAssignStr() { playerStats.AssignStatPoint("STR"); }
    public void OnAssignVit() { playerStats.AssignStatPoint("VIT"); }
    public void OnAssignDex() { playerStats.AssignStatPoint("DEX"); }
    public void OnAssignInt() { playerStats.AssignStatPoint("INT"); }
}