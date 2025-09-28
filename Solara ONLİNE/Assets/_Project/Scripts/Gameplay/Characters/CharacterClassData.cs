using UnityEngine;
using System.Collections.Generic;

// Hangi silah t�r�n�n hangi animasyon setini kullanaca��n� belirten yap�.
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

    [Header("Yetenek Gruplar�")]
    public List<SkillData> skillGroup1; // Bedensel, B�y�l� Kara vs.
    public List<SkillData> skillGroup2; // Zihinsel, Karab�y� vs.
    public List<SkillData> supportSkills; // At skilleri gibi
}