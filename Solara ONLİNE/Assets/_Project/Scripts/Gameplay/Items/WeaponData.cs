// WeaponData.cs
using UnityEngine;

// Artýk özel bir "Silah" oluþturabiliriz.
[CreateAssetMenu(fileName = "New Weapon", menuName = "Solara/Data/Items/Weapon")]
public class WeaponData : EquipmentData
{
    [Header("Silah Statlarý")]
    public int minDamage;
    public int maxDamage;
}