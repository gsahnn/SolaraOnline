// SkillHolder.cs
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
    // Oyuncunun sahip olduðu tüm yetenek slotlarý.
    public List<SkillSlot> skills = new List<SkillSlot>();

    // Yeteneklerin bekleme sürelerini güncellemek için event. UI bunu dinleyecek.
    public event Action OnSkillsUpdated;

    // Referanslar
    private CharacterStats myStats;

    private void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        // Her frame'de tüm yeteneklerin bekleme sürelerini azalt.
        bool cooldownUpdated = false;
        foreach (var skill in skills)
        {
            if (skill.IsOnCooldown())
            {
                skill.UpdateCooldown(Time.deltaTime);
                cooldownUpdated = true;
            }
        }

        // Eðer en az bir yeteneðin süresi azaldýysa, UI'a haber ver.
        if (cooldownUpdated)
        {
            OnSkillsUpdated?.Invoke();
        }
    }

    // Yeni bir yetenek öðrenmek için fonksiyon.
    public void LearnSkill(SkillData skillData)
    {
        if (skillData != null)
        {
            skills.Add(new SkillSlot(skillData));
            OnSkillsUpdated?.Invoke();
        }
    }

    // Yeteneði kullanmaya çalýþ.
    public void UseSkill(int skillIndex, MonsterController target)
    {
        // Geçerli bir yetenek mi?
        if (skillIndex < 0 || skillIndex >= skills.Count) return;

        SkillSlot skillSlot = skills[skillIndex];

        // Bekleme süresinde mi? Manamýz yeterli mi?
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

        // Tüm kontrollerden geçti. Yeteneði kullan!
        myStats.currentMana -= skillSlot.skillData.manaCost;
        skillSlot.PutOnCooldown();

        Debug.Log(skillSlot.skillData.skillName + " kullanýldý!");

        // Yeteneðin etkisini uygula (þimdilik sadece hasar)
        if (skillSlot.skillData.type == SkillType.Damage)
        {
            if (target != null && target.TryGetComponent(out CharacterStats targetStats))
            {
                int baseDamage = skillSlot.skillData.baseDamage;
                float multiplier = skillSlot.skillData.damageMultiplier;
                int finalDamage = baseDamage + Mathf.RoundToInt(myStats.intelligence * multiplier); // Büyü hasarý INT'e baðlý olsun.

                targetStats.TakeDamage(finalDamage, myStats);
            }
        }

        OnSkillsUpdated?.Invoke();
    }
}