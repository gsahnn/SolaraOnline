// CharacterStatsUI_Controller.cs (YEN� VE G�NCELLENM�� HAL�)
using UnityEngine;
using TMPro;

public class CharacterStatsUI_Controller : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    public static CharacterStatsUI_Controller Instance { get; private set; }
    // -------------------------

    [Header("Panel")]
    [SerializeField] private GameObject characterStatsPanel; // Panelin kendisini buraya atayaca��z.

    [Header("UI Metin Alanlar�")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    // ... (di�er text alanlar� ayn� kalacak) ...

    // ... (butonlar ayn� kalacak) ...

    private CharacterStats playerStats;

    private void Awake()
    {
        // Singleton'� kur
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Initialize(CharacterStats stats)
    {
        playerStats = stats;
        playerStats.OnStatsChanged += UpdateUI;
        UpdateUI();
    }

    // ... OnDestroy() fonksiyonu ayn� kalacak ...

    private void UpdateUI()
    {
        // ... UpdateUI() fonksiyonu ayn� kalacak ...
    }

    // 'C' tu�una bas�ld���nda paneli a�/kapat i�ini art�k PlayerController de�il,
    // bu script'in kendisi yaps�n. Bu daha d�zenli bir yap�.
    public void TogglePanel()
    {
        if (characterStatsPanel != null)
        {
            characterStatsPanel.SetActive(!characterStatsPanel.activeSelf);

            // Panel her a��ld���nda verilerin g�ncel oldu�undan emin ol.
            if (characterStatsPanel.activeSelf)
            {
                UpdateUI();
            }
        }
    }

    // Fonksiyon, panelin a��k olup olmad���n� PlayerController'a bildirecek.
    public bool IsOpen()
    {
        return characterStatsPanel != null && characterStatsPanel.activeSelf;
    }

    public void OnAssignStr() { playerStats.AssignStatPoint("STR"); }
    public void OnAssignVit() { playerStats.AssignStatPoint("VIT"); }
    public void OnAssignDex() { playerStats.AssignStatPoint("DEX"); }
    public void OnAssignInt() { playerStats.AssignStatPoint("INT"); }
    
    
}