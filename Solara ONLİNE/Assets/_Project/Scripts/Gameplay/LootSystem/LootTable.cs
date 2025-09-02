// LootTable.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Table", menuName = "Solara/Data/Loot Table")]
public class LootTable : ScriptableObject
{
    public List<LootItem> possibleLoot; // Bu tablodan düþebilecek tüm olasý eþyalar.
}