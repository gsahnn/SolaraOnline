// GameDataTypes.cs

using System;

// [Flags] özelliði, Inspector'da çoklu seçim yapmamýzý saðlar.
[Flags]
public enum CharacterClass
{
    None = 0,
    Warrior = 1 << 0,
    Ninja = 1 << 1,
    Sura = 1 << 2,
    Shaman = 1 << 3,
    Wolfman = 1 << 4
}

public enum WeaponType { Unarmed, OneHandedSword, TwoHandedSword, Dagger, Bow, Fan, Bell, Claw }

public enum EquipmentSlot { Weapon, Armor, Helmet, Boots, Shield, Necklace, Earrings, Bracelet }

public enum BonusType { MaxHP, MaxSP, Strength, Dexterity, Intelligence, Vitality, AttackSpeed, MovementSpeed }

// ... Gelecekteki diðer genel enum'lar buraya eklenebilir ...