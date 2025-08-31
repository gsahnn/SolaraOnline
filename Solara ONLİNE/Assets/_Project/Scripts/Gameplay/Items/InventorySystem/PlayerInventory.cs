// PlayerInventory.cs
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Oyuncunun envanter sistemini tutar. Inspector'dan boyutunu ayarlayabilece�iz.
    public InventorySystem inventory;

    private void Awake()
    {
        // Oyuncu yarat�ld���nda, 20 slotluk bir envanter olu�tur.
        inventory = new InventorySystem(20);
    }
}