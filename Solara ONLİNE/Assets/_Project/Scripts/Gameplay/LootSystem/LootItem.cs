// LootItem.cs
using System; // Serializable i�in
using UnityEngine;
[Serializable]
public class LootItem
{
    public ItemData itemData; // Hangi e�ya d��ecek?

    [Range(0.01f, 100f)] // Inspector'da 0.01 ile 100 aras�nda bir slider olu�turur.
    public float dropChance; // D��me �ans� (y�zde olarak)
}