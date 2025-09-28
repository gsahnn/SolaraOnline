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
    public CharacterClass usableByClasses; // Art�k GameDataTypes'tan geliyor.

    [Header("E�ya Ayarlar�")]
    public bool isStackable;
    public int maxStackSize = 1;
}