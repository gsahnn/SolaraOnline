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
            OnSkillsUpdated?.Invoke(); // UI'a bekleme s�relerinin g�ncellendi�ini bildir.
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

    // Parametre olarak art�k genel bir CharacterStats al�r. Bu, hem canavarlara hem de oyunculara yetenek kullanabilmeyi sa�lar.
    public void UseSkill(int skillIndex, CharacterStats targetStats)
    {
        if (skillIndex < 0 || skillIndex >= skills.Count) return;

        SkillSlot skillSlot = skills[skillIndex];

        if (skillSlot.IsOnCooldown())
        {
            Debug.Log(skillSlot.skillData.skillName + " bekleme s�resinde!");
            return;
        }

        if (myStats.currentMana < skillSlot.skillData.manaCost)
        {
            Debug.Log("Yeterli mana yok!");
            return;
        }

        myStats.currentMana -= skillSlot.skillData.manaCost;
        skillSlot.PutOnCooldown();

        Debug.Log(myStats.name + ", " + skillSlot.skillData.skillName + " yetene�ini " + (targetStats != null ? targetStats.name : "bo�lu�a") + " kulland�!");

        if (skillSlot.skillData.type == SkillType.Damage)
        {
            if (targetStats != null)
            {
                int baseDamage = skillSlot.skillData.baseDamage;
                float multiplier = skillSlot.skillData.damageMultiplier;
                int finalDamage = baseDamage + Mathf.RoundToInt(myStats.intelligence * multiplier);

                // Hasar� ve yere d���rme bilgisini hedefin CharacterStats'�na g�nder.
                targetStats.TakeDamage(finalDamage, myStats, skillSlot.skillData.causesKnockdown);
            }
        }

        OnSkillsUpdated?.Invoke();
    }
}