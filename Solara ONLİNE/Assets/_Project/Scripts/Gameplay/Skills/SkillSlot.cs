// Konum: _Project/Scripts/Gameplay/Skills/SkillSlot.cs

using System;
using UnityEngine; // SkillData i�in gerekli

// Bu s�n�f MonoBehaviour'dan t�remez, ��nk� sahnede bir obje de�ildir.
// Sadece veri tutan basit bir C# s�n�f�d�r.
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