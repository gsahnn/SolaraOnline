using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    // --- YEN� EKLENEN S�STEM ENTEGRASYONU ---
    [Header("Sistem Entegrasyonu")]
    [Tooltip("Proje klas�r�ndeki Nameplate Prefab'�n� buraya s�r�kleyin.")]
    [SerializeField] private GameObject nameplatePrefab;
    [Tooltip("Nameplate'in olu�turulaca�� pozisyon. Karakterin ba��n�n �zerinde bo� bir objedir.")]
    [SerializeField] private Transform nameplateAnchor;
    [Tooltip("Proje klas�r�ndeki RankDatabase SO'sunu buraya s�r�kleyin.")]
    public RankDatabase rankDatabase;

    // --- YEN� EKLENEN HEDEFLEME UI B�LG�LER� ---
    [Header("Hedefleme UI Bilgileri")]
    [Tooltip("Canavar�n g�c�n� belirten metin (1.Seviye, Patron, Elit vb.)")]
    public string characterGrade;
    [Tooltip("Karakterin �rk�n� veya tipini belirten metin.")]
    public string raceName;

    // --- MEVCUT K�ML�K B�LG�LER�N ---
    [Header("Kimlik Bilgileri")]
    public string characterName;
    public string guildName;

    // --- MEVCUT ��HRET VE DERECE S�STEM�N ---
    [Header("��hret ve Derece")]
    [Tooltip("Karakterin mevcut derece/��hret puan�.")]
    public int alignment = 0;

    // --- MEVCUT D��ER T�M STATLARIN KORUNDU ---
    #region Mevcut Statlar
    [Header("Ekonomi")]
    public int gold = 1000;
    [Header("Seviye ve Tecr�be")]
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
    [Header("Sava� Statlar�")]
    public int minDamage;
    public int maxDamage;
    public int defense;
    [Header("�d�ller (Sadece Canavarlar ��in)")]
    public int experienceGranted = 50;
    #endregion

    // --- G�NCELLENM�� EVENT S�STEM� ---
    public event Action<CharacterStats> OnDeath;
    public event Action OnDamageTaken;
    public event Action<CharacterStats> OnStatsInitialized; // Nameplate'in �LK kurulumu i�in.
    public event Action<CharacterStats> OnStatsChanged;     // Sonraki T�M g�ncellemeler i�in.

    private void Awake()
    {
        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;

        // Nameplate olu�turma sorumlulu�u art�k burada.
        CreateNameplate();
    }

    private void Start()
    {
        // T�m UI'lara "Ben haz�r�m, ilk bilgileri y�kleyin" sinyalini g�nder.
        OnStatsInitialized?.Invoke(this);
    }

    // --- YEN� NAMEPLATE OLU�TURMA FONKS�YONU ---
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
            Debug.LogError("Olu�turulan Nameplate prefab'�n�n i�inde Nameplate_Controller script'i bulunamad�!", nameplateInstance);
        }
    }

    // --- MEVCUT T�M FONKS�YONLARIN KORUNDU (DE����KL�K YOK) ---
    // Bu fonksiyonlar�n i�indeki OnStatsChanged �a�r�lar�n, UI'�n g�ncellenmesini zaten sa�l�yor.
    #region Mevcut Fonksiyonlar
    public void TakeDamage(int damage, CharacterStats attacker)
    {
        if (currentHealth <= 0) return;
        int damageTaken = Mathf.Max(damage - defense, 1);
        currentHealth -= damageTaken;
        OnDamageTaken?.Invoke();
        OnStatsChanged?.Invoke(this); // Sinyal g�nderiliyor.
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