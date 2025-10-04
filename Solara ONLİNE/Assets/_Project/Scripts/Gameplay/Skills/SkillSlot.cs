using System;
using UnityEngine;

// Bu bir class't�r, bir component DE��L. Bu y�zden MonoBehaviour'dan t�remez.
[System.Serializable]
public class SkillSlot
{
    public SkillData skillData;
    private float cooldownTimer;

    public SkillSlot(SkillData data)
    {
        skillData = data;
        cooldownTimer = 0;
    }

    public bool IsOnCooldown()
    {
        return cooldownTimer > 0;
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= deltaTime;
        }
    }

    public void PutOnCooldown()
    {
        cooldownTimer = skillData.cooldown;
    }

    // UI'da bekleme s�resini dairesel (radial) bir �ekilde g�stermek i�in kullan��l�d�r.
    public float GetCooldownProgress()
    {
        if (skillData == null || skillData.cooldown <= 0) return 0;
        return cooldownTimer / skillData.cooldown;
    }
}