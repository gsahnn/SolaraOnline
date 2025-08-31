// WeaponData.cs
using UnityEngine;

// Art�k �zel bir "Silah" olu�turabiliriz.
[CreateAssetMenu(fileName = "New Weapon", menuName = "Solara/Data/Items/Weapon")]
public class WeaponData : EquipmentData
{
    [Header("Silah Statlar�")]
    public int minDamage;
    public int maxDamage;
}