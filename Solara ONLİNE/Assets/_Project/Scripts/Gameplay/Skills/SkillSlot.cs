// Konum: _Project/Scripts/Gameplay/Skills/SkillSlot.cs

using System;
using UnityEngine; // SkillData için gerekli

// Bu sýnýf MonoBehaviour'dan türemez, çünkü sahnede bir obje deðildir.
// Sadece veri tutan basit bir C# sýnýfýdýr.
[Serializable]
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