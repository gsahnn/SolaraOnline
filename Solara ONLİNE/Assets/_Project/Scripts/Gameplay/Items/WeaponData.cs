using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Solara/Data/Items/Weapon")]
public class WeaponData : EquipmentData
{
    [Header("Silah Ayarlar�")]
    public WeaponType weaponType;
    public int minDamage;
    public int maxDamage;
}