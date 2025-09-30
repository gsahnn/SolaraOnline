using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    [Header("Sistem Entegrasyonu")]
    [Tooltip("Proje klas�r�ndeki Nameplate Prefab'�n� buraya s�r�kleyin.")]
    [SerializeField] private GameObject nameplatePrefab;
    [Tooltip("Nameplate'in olu�turulaca�� pozisyon. Karakterin ba��n�n �zerinde bo� bir objedir.")]
    [SerializeField] private Transform nameplateAnchor;
    [Tooltip("Proje klas�r�ndeki RankDatabase SO'sunu buraya s�r�kleyin.")]
    public RankDatabase rankDatabase;

    [Header("Kimlik Bilgileri")]
    public string characterName;
    public string guildName;
    [Tooltip("Karakterin mevcut derece/��hret puan�.")]
    public int alignment = 0;

    // ... (Di�er t�m stat de�i�kenlerin burada yer al�yor, de�i�tirmeye gerek yok)
    #region Di�er De�i�kenler
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

    public event Action<CharacterStats> OnDeath;
    public event Action OnDamageTaken;
    public event Action<CharacterStats> OnStatsInitialized;
    public event Action<CharacterStats> OnStatsChanged;

    private void Awake()
    {
        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
        CreateNameplate();
    }

    private void Start()
    {
        OnStatsInitialized?.Invoke(this);
    }

    private void CreateNameplate()
    {
        if (nameplatePrefab == null || nameplateAnchor == null)
        {
            if (CompareTag("Player"))
                Debug.LogError("Player i�in Nameplate Prefab veya Nameplate Anchor atanmam��!", this.gameObject);
            return;
        }

        GameObject nameplateInstance = Instantiate(nameplatePrefab, nameplateAnchor.position, nameplateAnchor.rotation, nameplateAnchor);
        nameplateInstance.GetComponent<Nameplate_Controller>()?.Initialize(this);
    }

    // --- Test i�in Update Fonksiyonu ---
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeAlignment(UnityEngine.Random.Range(-500, 500));
        }
    }

    // ... (TakeDamage, Die, CalculateAllStats ve di�er t�m fonksiyonlar�n burada yer al�yor, de�i�tirmeye gerek yok)
    #region Mevcut Fonksiyonlar
    public void TakeDamage(int damage, CharacterStats attacker)
    {
        if (currentHealth <= 0) return;
        int damageTaken = Mathf.Max(damage - defense, 1);
        currentHealth -= damageTaken;
        OnDamageTaken?.Invoke();
        OnStatsChanged?.Invoke(this);
        if (currentHealth <= 0) { currentHealth = 0; Die(attacker); }
    }
    private void Die(CharacterStats killer) { OnDeath?.Invoke(killer); }
    public void CalculateAllStats()
    {
        maxHealth = vitality * 10;
        maxMana = intelligence * 10;
        minDamage = strength;
        maxDamage = strength + 4;
        defense = dexterity / 2;
        OnStatsChanged?.Invoke(this);
    }
    public void AddExperience(int amount)
    {
        currentExp += amount;
        while (currentExp >= expToNextLevel) { currentExp -= expToNextLevel; LevelUp(); }
        OnStatsChanged?.Invoke(this);
    }
    private void LevelUp()
    {
        level++;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);
        statPointsToAssign += 5;
        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
        Debug.Log(transform.name + " SEV�YE ATLADI! Yeni seviye: " + level);
    }
    public void AssignStatPoint(string statName)
    {
        if (statPointsToAssign > 0)
        {
            statPointsToAssign--;
            switch (statName.ToUpper())
            {
                case "STR": strength++; break;
                case "DEX": dexterity++; break;
                case "VIT": vitality++; break;
                case "INT": intelligence++; break;
            }
            CalculateAllStats();
        }
    }
    public void ChangeAlignment(int amount)
    {
        alignment += amount;
        alignment = Mathf.Clamp(alignment, -20000, 20000);
        Debug.Log("Derece puan� de�i�ti. Yeni puan: " + alignment);
        OnStatsChanged?.Invoke(this);
    }
    #endregion
}