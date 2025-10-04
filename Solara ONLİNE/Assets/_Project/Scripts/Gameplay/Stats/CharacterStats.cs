using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    // --- N�HA� D�ZELTME: Animator referans� en �ste ta��nd� ---
    private Animator animator;

    #region De�i�kenler
    [Header("Sistem Entegrasyonu")]
    [SerializeField] private GameObject nameplatePrefab;
    [SerializeField] private Transform nameplateAnchor;
    public RankDatabase rankDatabase;
    [Header("Hedefleme UI Bilgileri")]
    public string characterGrade;
    public string raceName;
    [Header("Kimlik Bilgileri")]
    public string characterName;
    public string guildName;
    [Header("��hret ve Derece")]
    public int alignment = 0;
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

    #region Eventler
    public event Action<CharacterStats> OnDeath;
    public event Action OnDamageTaken;
    public event Action<CharacterStats> OnStatsInitialized;
    public event Action<CharacterStats> OnStatsChanged;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Referans� en ba�ta, bir kere al�yoruz.
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
        if (nameplatePrefab == null || nameplateAnchor == null) return;
        GameObject nameplateInstance = Instantiate(nameplatePrefab, nameplateAnchor.position, nameplateAnchor.rotation, nameplateAnchor);
        Nameplate_Controller controller = nameplateInstance.GetComponentInChildren<Nameplate_Controller>();
        if (controller != null) { controller.Initialize(this); }
    }

    // --- N�HA� D�ZELTME: HASAR ALMA MANTI�I ---
    public void TakeDamage(int damage, CharacterStats attacker, bool causesKnockdown)
    {
        if (currentHealth <= 0) return;

        currentHealth -= Mathf.Max(damage - defense, 1);
        OnDamageTaken?.Invoke();
        OnStatsChanged?.Invoke(this);

        if (animator == null) return;

        // 1. E�er sald�r� yere d���r�yorsa, di�er her �eyi iptal et ve d��.
        if (causesKnockdown)
        {
            Vector3 directionFromAttacker = transform.position - attacker.transform.position;
            float dotProduct = Vector3.Dot(transform.forward, directionFromAttacker.normalized);

            if (dotProduct < 0) { animator.SetTrigger("Knockdown_Front"); }
            else { animator.SetTrigger("Knockdown_Back"); }
        }
        // 2. E�er yere d��m�yorsa VE o an bir sald�r� animasyonu OYNATMIYORSA, normal hasar animasyonunu oyna.
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            animator.SetTrigger("TakeDamage");
        }
        // 3. (�nemli) E�er sald�r� animasyonu oynat�l�yorsa, bu blok hi�bir �ey yapmaz ve sald�r� b�l�nmez.

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(attacker);
        }
    }

    #region Di�er T�m Fonksiyonlar (Eksiksiz)
    private void Die(CharacterStats killer) { OnDeath?.Invoke(killer); animator.SetTrigger("Die"); }
    public void CalculateAllStats() { maxHealth = vitality * 10; maxMana = intelligence * 10; minDamage = strength; maxDamage = strength + 4; defense = dexterity / 2; OnStatsChanged?.Invoke(this); }
    public void AddExperience(int amount) { currentExp += amount; while (currentExp >= expToNextLevel) { currentExp -= expToNextLevel; LevelUp(); } OnStatsChanged?.Invoke(this); }
    private void LevelUp() { level++; expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f); statPointsToAssign += 5; CalculateAllStats(); currentHealth = maxHealth; currentMana = maxMana; }
    public void AssignStatPoint(string statName) { if (statPointsToAssign > 0) { statPointsToAssign--; switch (statName.ToUpper()) { case "STR": strength++; break; case "DEX": dexterity++; break; case "VIT": vitality++; break; case "INT": intelligence++; break; } CalculateAllStats(); } }
    public void ChangeAlignment(int amount) { alignment += amount; alignment = Mathf.Clamp(alignment, -20000, 20000); OnStatsChanged?.Invoke(this); }
    #endregion
}