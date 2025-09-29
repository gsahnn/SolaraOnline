using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterStats : MonoBehaviour
{
    [Header("UI Referansý")]
    [SerializeField] private GameObject nameplatePrefab;

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

    // EVENT'LER: Diðer sistemler bu olaylarý dinler.
    public event Action<CharacterStats> OnDeath;
    public event Action OnDamageTaken;
    public event Action<CharacterStats> OnStatsChanged;

    private void Awake()
    {
        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Start()
    {
        if (nameplatePrefab != null)
        {
            GameObject nameplateInstance = Instantiate(nameplatePrefab, transform);
            nameplateInstance.transform.localPosition = new Vector3(0, 2.2f, 0);
            nameplateInstance.GetComponent<Nameplate_Controller>()?.Initialize(this);
        }
        OnStatsChanged?.Invoke(this);
    }

    public void TakeDamage(int damage, CharacterStats attacker)
    {
        if (currentHealth <= 0) return;
        int damageTaken = Mathf.Max(damage - defense, 1);
        currentHealth -= damageTaken;

        OnDamageTaken?.Invoke();
        OnStatsChanged?.Invoke(this);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(attacker);
        }
    }

    private void Die(CharacterStats killer)
    {
        OnDeath?.Invoke(killer);
    }

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
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }
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
        Debug.Log(transform.name + " SEVÝYE ATLADI! Yeni seviye: " + level);
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
}