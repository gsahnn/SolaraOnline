// CharacterStatsUI_Controller.cs (YENÝ VE GÜNCELLENMÝÞ HALÝ)
using UnityEngine;
using TMPro;

public class CharacterStatsUI_Controller : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    public static CharacterStatsUI_Controller Instance { get; private set; }
    // -------------------------

    [Header("Panel")]
    [SerializeField] private GameObject characterStatsPanel; // Panelin kendisini buraya atayacaðýz.

    [Header("UI Metin Alanlarý")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    // ... (diðer text alanlarý ayný kalacak) ...

    // ... (butonlar ayný kalacak) ...

    private CharacterStats playerStats;

    private void Awake()
    {
        // Singleton'ý kur
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

    // ... OnDestroy() fonksiyonu ayný kalacak ...

    private void UpdateUI()
    {
        // ... UpdateUI() fonksiyonu ayný kalacak ...
    }

    // 'C' tuþuna basýldýðýnda paneli aç/kapat iþini artýk PlayerController deðil,
    // bu script'in kendisi yapsýn. Bu daha düzenli bir yapý.
    public void TogglePanel()
    {
        if (characterStatsPanel != null)
        {
            characterStatsPanel.SetActive(!characterStatsPanel.activeSelf);

            // Panel her açýldýðýnda verilerin güncel olduðundan emin ol.
            if (characterStatsPanel.activeSelf)
            {
                UpdateUI();
            }
        }
    }

    // Fonksiyon, panelin açýk olup olmadýðýný PlayerController'a bildirecek.
    public bool IsOpen()
    {
        return characterStatsPanel != null && characterStatsPanel.activeSelf;
    }

    public void OnAssignStr() { playerStats.AssignStatPoint("STR"); }
    public void OnAssignVit() { playerStats.AssignStatPoint("VIT"); }
    public void OnAssignDex() { playerStats.AssignStatPoint("DEX"); }
    public void OnAssignInt() { playerStats.AssignStatPoint("INT"); }
    
    
}