// SkillData.cs
using UnityEngine;

// Yetene�imizin t�r�n� belirlemek i�in bir enum. �imdilik sadece hasar verenler var.
public enum SkillType
{
    Damage,
    Heal,
    Buff
}

[CreateAssetMenu(fileName = "New Skill", menuName = "Solara/Data/Skill")]
public class SkillData : ScriptableObject
{
    [Header("Temel Bilgiler")]
    public string skillName;
    public Sprite icon;
    [TextArea(4, 8)]
    public string description;

    [Header("Yetenek Ayarlar�")]
    public SkillType type;
    public int manaCost; // Ne kadar mana harcayacak?
    public float cooldown; // Bekleme s�resi (saniye cinsinden)
    public float castTime; // Yetene�i kullanma s�resi (animasyonla senkronize olacak)

    [Header("Hasar Ayarlar� (E�er t�r� Damage ise)")]
    public int baseDamage; // Yetene�in temel hasar�
    public float damageMultiplier; // Oyuncunun g�c�nden (STR/INT) ne kadar etkilenecek?

    // �leride buraya yetenek animasyonu, ses efekti, g�rsel efekt (VFX) gibi alanlar da ekleyece�iz.
}