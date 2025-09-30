using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    // --- YENÝ EKLENEN SÝSTEM ENTEGRASYONU ---
    [Header("Sistem Entegrasyonu")]
    [Tooltip("Proje klasöründeki Nameplate Prefab'ýný buraya sürükleyin.")]
    [SerializeField] private GameObject nameplatePrefab;
    [Tooltip("Nameplate'in oluþturulacaðý pozisyon. Karakterin baþýnýn üzerinde boþ bir objedir.")]
    [SerializeField] private Transform nameplateAnchor;
    [Tooltip("Proje klasöründeki RankDatabase SO'sunu buraya sürükleyin.")]
    public RankDatabase rankDatabase;

    // --- YENÝ EKLENEN HEDEFLEME UI BÝLGÝLERÝ ---
    [Header("Hedefleme UI Bilgileri")]
    [Tooltip("Canavarýn gücünü belirten metin (1.Seviye, Patron, Elit vb.)")]
    public string characterGrade;
    [Tooltip("Karakterin ýrkýný veya tipini belirten metin.")]
    public string raceName;

    // --- MEVCUT KÝMLÝK BÝLGÝLERÝN ---
    [Header("Kimlik Bilgileri")]
    public string characterName;
    public string guildName;

    // --- MEVCUT ÞÖHRET VE DERECE SÝSTEMÝN ---
    [Header("Þöhret ve Derece")]
    [Tooltip("Karakterin mevcut derece/þöhret puaný.")]
    public int alignment = 0;

    // --- MEVCUT DÝÐER TÜM STATLARIN KORUNDU ---
    #region Mevcut Statlar
    [Header("Ekonomi")]
    public int gold = 1000;
    [Header("Seviye ve Tecrübe")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    public int statPointsToAssign = 0;
    [Header("Ana Statlar")]
    public int strength = 10;
    public int dexterity = 10;
    public int intelligence = 10;
    public int vitality = 10;
    [Header("Can ve Mana")]
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    [Header("Savaþ Statlarý")]
    public int minDamage;
    public int maxDamage;
    public int defense;
    [Header("Ödüller (Sadece Canavarlar Ýçin)")]
    public int experienceGranted = 50;
    #endregion

    // --- GÜNCELLENMÝÞ EVENT SÝSTEMÝ ---
    public event Action<CharacterStats> OnDeath;
    public event Action OnDamageTaken;
    public event Action<CharacterStats> OnStatsInitialized; // Nameplate'in ÝLK kurulumu için.
    public event Action<CharacterStats> OnStatsChanged;     // Sonraki TÜM güncellemeler için.

    private void Awake()
    {
        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;

        // Nameplate oluþturma sorumluluðu artýk burada.
        CreateNameplate();
    }

    private void Start()
    {
        // Tüm UI'lara "Ben hazýrým, ilk bilgileri yükleyin" sinyalini gönder.
        OnStatsInitialized?.Invoke(this);
    }

    // --- YENÝ NAMEPLATE OLUÞTURMA FONKSÝYONU ---
    private void CreateNameplate()
    {
        if (nameplatePrefab == null || nameplateAnchor == null) return;

        GameObject nameplateInstance = Instantiate(nameplatePrefab, nameplateAnchor.position, nameplateAnchor.rotation, nameplateAnchor);
        Nameplate_Controller controller = nameplateInstance.GetComponentInChildren<Nameplate_Controller>();

        if (controller != null)
        {
            controller.Initialize(this);
        }
        else
        {
            Debug.LogError("Oluþturulan Nameplate prefab'ýnýn içinde Nameplate_Controller script'i bulunamadý!", nameplateInstance);
        }
    }

    // --- MEVCUT TÜM FONKSÝYONLARIN KORUNDU (DEÐÝÞÝKLÝK YOK) ---
    // Bu fonksiyonlarýn içindeki OnStatsChanged çaðrýlarýn, UI'ýn güncellenmesini zaten saðlýyor.
    #region Mevcut Fonksiyonlar
    public void TakeDamage(int damage, CharacterStats attacker)
    {
        if (currentHealth <= 0) return;
        int damageTaken = Mathf.Max(damage - defense, 1);
        currentHealth -= damageTaken;
        OnDamageTaken?.Invoke();
        OnStatsChanged?.Invoke(this); // Sinyal gönderiliyor.
        if (currentHealth <= 0) { currentHealth = 0; Die(attacker); }
    }
    private void Die(CharacterStats killer) { OnDeath?.Invoke(killer); }
    public void CalculateAllStats() { maxHealth = vitality * 10; maxMana = intelligence * 10; minDamage = strength; maxDamage = strength + 4; defense = dexterity / 2; OnStatsChanged?.Invoke(this); }
    public void AddExperience(int amount) { currentExp += amount; while (currentExp >= expToNextLevel) { currentExp -= expToNextLevel; LevelUp(); } OnStatsChanged?.Invoke(this); }
    private void LevelUp() { level++; expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f); statPointsToAssign += 5; CalculateAllStats(); currentHealth = maxHealth; currentMana = maxMana; }
    public void AssignStatPoint(string statName) { if (statPointsToAssign > 0) { statPointsToAssign--; switch (statName.ToUpper()) { case "STR": strength++; break; case "DEX": dexterity++; break; case "VIT": vitality++; break; case "INT": intelligence++; break; } CalculateAllStats(); } }
    public void ChangeAlignment(int amount) { alignment += amount; alignment = Mathf.Clamp(alignment, -20000, 20000); OnStatsChanged?.Invoke(this); }
    #endregion
}