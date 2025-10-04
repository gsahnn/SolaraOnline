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
        foreach (var skillSlot in skills)
        {
            if (skillSlot.IsOnCooldown())
            {
                skillSlot.UpdateCooldown(Time.deltaTime);
                cooldownUpdated = true;
            }
        }
        if (cooldownUpdated)
        {
            OnSkillsUpdated?.Invoke(); // UI'a bekleme sürelerinin güncellendiðini bildir.
        }
    }

    public void LearnSkill(SkillData skillData)
    {
        if (skillData != null && !skills.Any(s => s.skillData == skillData))
        {
            skills.Add(new SkillSlot(skillData));
            OnSkillsUpdated?.Invoke();
        }
    }

    // Parametre olarak artýk genel bir CharacterStats alýr. Bu, hem canavarlara hem de oyunculara yetenek kullanabilmeyi saðlar.
    public void UseSkill(int skillIndex, CharacterStats targetStats)
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

        Debug.Log(myStats.name + ", " + skillSlot.skillData.skillName + " yeteneðini " + (targetStats != null ? targetStats.name : "boþluða") + " kullandý!");

        if (skillSlot.skillData.type == SkillType.Damage)
        {
            if (targetStats != null)
            {
                int baseDamage = skillSlot.skillData.baseDamage;
                float multiplier = skillSlot.skillData.damageMultiplier;
                int finalDamage = baseDamage + Mathf.RoundToInt(myStats.intelligence * multiplier);

                // Hasarý ve yere düþürme bilgisini hedefin CharacterStats'ýna gönder.
                targetStats.TakeDamage(finalDamage, myStats, skillSlot.skillData.causesKnockdown);
            }
        }

        OnSkillsUpdated?.Invoke();
    }
}