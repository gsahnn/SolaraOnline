using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
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
    public int experienceGranted = 50; // �d�l EXP'si buraya ta��nd�.

    // --- YEN� EKLENEN EVENT'LER ---
    // MonsterController bu olaylar� dinleyecek.
    public event Action<CharacterStats> OnDeath;
    public event Action OnDamageTaken;
    // --------------------------------

    public event Action OnStatsChanged;

    private void Awake()
    {
        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    public void TakeDamage(int damage, CharacterStats attacker)
    {
        if (currentHealth <= 0) return; // Zaten �lm��se tekrar hasar alma.

        int damageTaken = Mathf.Max(damage - defense, 1);
        currentHealth -= damageTaken;

        // Hasar al�nd���nda OnDamageTaken event'ini tetikle.
        OnDamageTaken?.Invoke();
        OnStatsChanged?.Invoke(); // UI g�ncellemeleri i�in

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(attacker);
        }
    }

    private void Die(CharacterStats killer)
    {
        // �l�m event'ini tetikle.
        OnDeath?.Invoke(killer);
    }

    // De�i�meyen di�er t�m fonksiyonlar
    #region Unchanged Methods
    public void CalculateAllStats()
    {
        maxHealth = vitality * 10;
        maxMana = intelligence * 10;
        minDamage = strength;
        maxDamage = strength + 4;
        defense = dexterity / 2;
        OnStatsChanged?.Invoke();
    }
    public void AddExperience(int amount)
    {
        currentExp += amount;
        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
        OnStatsChanged?.Invoke();
    }
    private void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);
        statPointsToAssign += 5;
        vitality += 2;
        strength += 2;
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
    #endregion
}