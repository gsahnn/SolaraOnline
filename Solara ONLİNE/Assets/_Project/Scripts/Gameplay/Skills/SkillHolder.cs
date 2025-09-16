using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class SkillHolder : MonoBehaviour
{
    public List<SkillSlot> skills = new List<SkillSlot>();
    public event Action OnSkillsUpdated;

    private CharacterStats myStats;

    private void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        bool cooldownUpdated = false;
        foreach (var skill in skills)
        {
            if (skill.IsOnCooldown())
            {
                skill.UpdateCooldown(Time.deltaTime);
                cooldownUpdated = true;
            }
        }
        if (cooldownUpdated)
        {
            OnSkillsUpdated?.Invoke();
        }
    }

    public void LearnSkill(SkillData skillData)
    {
        if (skillData != null && !skills.Any(s => s.skillData == skillData))
        {
            skills.Add(new SkillSlot(skillData));
            Debug.Log("<color=green>BAÞARILI:</color> " + skillData.name + " yeteneði SkillHolder'a eklendi.");
            OnSkillsUpdated?.Invoke();
        }
    }

    public void UseSkill(int skillIndex, MonsterController target)
    {
        if (skillIndex < 0 || skillIndex >= skills.Count) return;

        SkillSlot skillSlot = skills[skillIndex];

        if (skillSlot.IsOnCooldown())
        {
            Debug.Log(skillSlot.skillData.skillName + " bekleme süresinde!");
            return;
        }

        if (myStats.currentMana < skillSlot.skillData.manaCost)
        {
            Debug.Log("Yeterli mana yok!");
            return;
        }

        myStats.currentMana -= skillSlot.skillData.manaCost;
        skillSlot.PutOnCooldown();

        Debug.Log(skillSlot.skillData.skillName + " kullanýldý!");

        if (skillSlot.skillData.type == SkillType.Damage)
        {
            if (target != null && target.TryGetComponent(out CharacterStats targetStats))
            {
                int baseDamage = skillSlot.skillData.baseDamage;
                float multiplier = skillSlot.skillData.damageMultiplier;
                int finalDamage = baseDamage + Mathf.RoundToInt(myStats.intelligence * multiplier);
                targetStats.TakeDamage(finalDamage, myStats);
            }
        }

        OnSkillsUpdated?.Invoke();
    }
}