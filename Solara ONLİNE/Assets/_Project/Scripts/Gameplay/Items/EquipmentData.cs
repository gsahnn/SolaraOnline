using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Bonus
{
    public BonusType type;
    public float value;
}

public class EquipmentData : ItemData
{
    [Header("Ekipman Ayarlarý")]
    public EquipmentSlot equipmentSlot;
    public List<Bonus> bonuses;
}