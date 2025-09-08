// SkillData.cs
using UnityEngine;

// Yeteneðimizin türünü belirlemek için bir enum. Þimdilik sadece hasar verenler var.
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

    [Header("Yetenek Ayarlarý")]
    public SkillType type;
    public int manaCost; // Ne kadar mana harcayacak?
    public float cooldown; // Bekleme süresi (saniye cinsinden)
    public float castTime; // Yeteneði kullanma süresi (animasyonla senkronize olacak)

    [Header("Hasar Ayarlarý (Eðer türü Damage ise)")]
    public int baseDamage; // Yeteneðin temel hasarý
    public float damageMultiplier; // Oyuncunun gücünden (STR/INT) ne kadar etkilenecek?

    // Ýleride buraya yetenek animasyonu, ses efekti, görsel efekt (VFX) gibi alanlar da ekleyeceðiz.
}