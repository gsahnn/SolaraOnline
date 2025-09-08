// SkillHolder.cs
using UnityEngine;
using System.Collections.Generic;
using System;

// Tek bir yetene�in durumunu tutan s�n�f (verisi ve bekleme s�resi)
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
    // Oyuncunun sahip oldu�u t�m yetenek slotlar�.
    public List<SkillSlot> skills = new List<SkillSlot>();

    // Yeteneklerin bekleme s�relerini g�ncellemek i�in event. UI bunu dinleyecek.
    public event Action OnSkillsUpdated;

    // Referanslar
    private CharacterStats myStats;

    private void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        // Her frame'de t�m yeteneklerin bekleme s�relerini azalt.
        bool cooldownUpdated = false;
        foreach (var skill in skills)
        {
            if (skill.IsOnCooldown())
            {
                skill.UpdateCooldown(Time.deltaTime);
                cooldownUpdated = true;
            }
        }

        // E�er en az bir yetene�in s�resi azald�ysa, UI'a haber ver.
        if (cooldownUpdated)
        {
            OnSkillsUpdated?.Invoke();
        }
    }

    // Yeni bir yetenek ��renmek i�in fonksiyon.
    public void LearnSkill(SkillData skillData)
    {
        if (skillData != null)
        {
            skills.Add(new SkillSlot(skillData));
            OnSkillsUpdated?.Invoke();
        }
    }

    // Yetene�i kullanmaya �al��.
    public void UseSkill(int skillIndex, MonsterController target)
    {
        // Ge�erli bir yetenek mi?
        if (skillIndex < 0 || skillIndex >= skills.Count) return;

        SkillSlot skillSlot = skills[skillIndex];

        // Bekleme s�resinde mi? Manam�z yeterli mi?
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

        // T�m kontrollerden ge�ti. Yetene�i kullan!
        myStats.currentMana -= skillSlot.skillData.manaCost;
        skillSlot.PutOnCooldown();

        Debug.Log(skillSlot.skillData.skillName + " kullan�ld�!");

        // Yetene�in etkisini uygula (�imdilik sadece hasar)
        if (skillSlot.skillData.type == SkillType.Damage)
        {
            if (target != null && target.TryGetComponent(out CharacterStats targetStats))
            {
                int baseDamage = skillSlot.skillData.baseDamage;
                float multiplier = skillSlot.skillData.damageMultiplier;
                int finalDamage = baseDamage + Mathf.RoundToInt(myStats.intelligence * multiplier); // B�y� hasar� INT'e ba�l� olsun.

                targetStats.TakeDamage(finalDamage, myStats);
            }
        }

        OnSkillsUpdated?.Invoke();
    }
}