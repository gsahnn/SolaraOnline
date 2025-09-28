using UnityEngine;

public class ItemData : ScriptableObject
{
    [Header("Temel Bilgiler")]
    public string itemName;
    public Sprite icon;
    [TextArea(4, 8)] public string description;
    public int sellPrice = 1;

    [Header("Gereksinimler")]
    public int requiredLevel = 1;
    public CharacterClass usableByClasses; // Artýk GameDataTypes'tan geliyor.

    [Header("Eþya Ayarlarý")]
    public bool isStackable;
    public int maxStackSize = 1;
}