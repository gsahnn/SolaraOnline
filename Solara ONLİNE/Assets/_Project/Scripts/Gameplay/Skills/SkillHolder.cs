using UnityEngine;
using System.Collections.Generic;
using System;

// Tek bir yeteneðin durumunu tutan sýnýf (verisi ve bekleme süresi)
public class SkillSlot
{
    public SkillData skillData;
    public float cooldownTimer;

    public SkillSlot(SkillData data)
    {
        skillData = data;
        cooldownTimer = 0;
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= deltaTime;
        }
    }

    public bool IsOnCooldown()
    {
        return cooldownTimer > 0;
    }

    public void PutOnCooldown()
    {
        cooldownTimer = skillData.cooldown;
    }
}


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

    // Yeni bir yetenek öðrenmek için fonksiyon.
    public void LearnSkill(SkillData skillData)
    {
        // Ayný yeteneðin zaten öðrenilip öðrenilmediðini kontrol et
        if (skillData != null && !skills.Exists(s => s.skillData == skillData))
        {
            skills.Add(new SkillSlot(skillData));

            // ÖNEMLÝ DEÐÝÞÝKLÝK: Yeni bir yetenek eklendiðinde UI'a anýnda haber veriyoruz.
            OnSkillsUpdated?.Invoke();
        }
    }

    // Yeteneði kullanmaya çalýþ.
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
                // Yetenek hasarý Zeka'ya (INT) baðlý olsun.
                int finalDamage = baseDamage + Mathf.RoundToInt(myStats.intelligence * multiplier);

                targetStats.TakeDamage(finalDamage, myStats);
            }
        }

        OnSkillsUpdated?.Invoke();
    }
}