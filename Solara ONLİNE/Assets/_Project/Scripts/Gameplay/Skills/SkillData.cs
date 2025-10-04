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

    // --- YEN� EKLENEN ALAN ---
    [Tooltip("Bu yetenek hedefe isabet etti�inde onu yere d���r�r m�?")]
    public bool causesKnockdown = false;
}