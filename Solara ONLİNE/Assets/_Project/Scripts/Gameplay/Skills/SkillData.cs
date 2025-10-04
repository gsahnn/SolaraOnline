using UnityEngine;

public enum SkillType { Damage, Heal, Buff }

[CreateAssetMenu(fileName = "New Skill", menuName = "Solara/Data/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public float cooldown;
    public int manaCost;
    public SkillType type;

    [Header("Damage Skill Properties")]
    public int baseDamage;
    public float damageMultiplier;

    // --- YENÝ EKLENEN ALAN ---
    [Tooltip("Bu yetenek hedefe isabet ettiðinde onu yere düþürür mü?")]
    public bool causesKnockdown = false;
}