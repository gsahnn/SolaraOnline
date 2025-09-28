using UnityEngine;
using System.Collections.Generic;

// Hangi silah türünün hangi animasyon setini kullanacaðýný belirten yapý.
[System.Serializable]
public class AnimationSetEntry
{
    public WeaponType weaponType;
    public AnimatorOverrideController animatorOverrideController;
}

[CreateAssetMenu(fileName = "New Class", menuName = "Solara/Data/Characters/Class")]
public class CharacterClassData : ScriptableObject
{
    public string className;

    [Header("Animasyon Setleri")]
    public List<AnimationSetEntry> animationSets;

    [Header("Yetenek Gruplarý")]
    public List<SkillData> skillGroup1; // Bedensel, Büyülü Kara vs.
    public List<SkillData> skillGroup2; // Zihinsel, Karabüyü vs.
    public List<SkillData> supportSkills; // At skilleri gibi
}