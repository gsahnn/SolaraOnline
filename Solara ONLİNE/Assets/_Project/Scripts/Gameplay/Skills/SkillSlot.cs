using System;
using UnityEngine;

// Bu bir class'týr, bir component DEÐÝL. Bu yüzden MonoBehaviour'dan türemez.
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

    // UI'da bekleme süresini dairesel (radial) bir þekilde göstermek için kullanýþlýdýr.
    public float GetCooldownProgress()
    {
        if (skillData == null || skillData.cooldown <= 0) return 0;
        return cooldownTimer / skillData.cooldown;
    }
}