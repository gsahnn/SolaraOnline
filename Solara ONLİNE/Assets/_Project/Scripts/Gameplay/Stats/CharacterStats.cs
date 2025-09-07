// CharacterStats.cs
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    [Header("Seviye ve Tecrübe")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

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

    public event Action OnStatsChanged;

    private void Awake()
    {
        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

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
        Debug.Log(transform.name + " " + amount + " EXP kazandý.");

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

        vitality += 2;
        strength += 2;

        CalculateAllStats();
        currentHealth = maxHealth;
        currentMana = maxMana;

        Debug.Log(transform.name + " SEVÝYE ATLADI! Yeni seviye: " + level);
    }

    public void TakeDamage(int damage, CharacterStats attacker)
    {
        int damageTaken = Mathf.Max(damage - defense, 1);
        currentHealth -= damageTaken;

        Debug.Log(transform.name + ", " + attacker.name + "'dan " + damageTaken + " hasar aldý. Kalan can: " + currentHealth);
        OnStatsChanged?.Invoke();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(attacker);
        }
    }

    private void Die(CharacterStats killer)
    {
        Debug.Log(transform.name + " öldü.");

        if (TryGetComponent(out MonsterController monster))
        {
            monster.HandleDeath(killer);
        }
        else if (TryGetComponent(out PlayerController player))
        {
            Debug.Log("OYUNCU ÖLDÜ!");
        }
    }
}