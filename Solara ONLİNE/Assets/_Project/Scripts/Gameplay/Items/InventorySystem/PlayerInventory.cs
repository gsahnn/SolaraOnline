// PlayerInventory.cs
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Oyuncunun envanter sistemini tutar. Inspector'dan boyutunu ayarlayabileceðiz.
    public InventorySystem inventory;

    private void Awake()
    {
        // Oyuncu yaratýldýðýnda, 20 slotluk bir envanter oluþtur.
        inventory = new InventorySystem(20);
    }
}