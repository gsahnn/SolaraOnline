// LootItem.cs
using System; // Serializable için
using UnityEngine;
[Serializable]
public class LootItem
{
    public ItemData itemData; // Hangi eþya düþecek?

    [Range(0.01f, 100f)] // Inspector'da 0.01 ile 100 arasýnda bir slider oluþturur.
    public float dropChance; // Düþme þansý (yüzde olarak)
}