using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => inventorySlots.Count;

    public Action<InventorySlot> OnInventorySlotChanged { get; internal set; }

    // Bu event, envantere eþya eklendiðinde veya envanterden eþya silindiðinde tetiklenir.
    public event Action OnInventoryUpdated;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(ItemData item, int amount)
    {
        // Biriktirilebiliyorsa mevcut bir slota eklemeyi dene
        if (item.isStackable)
        {
            InventorySlot slotToStack = FindSlotForStacking(item);
            if (slotToStack != null)
            {
                slotToStack.AddToStack(amount);
                OnInventoryUpdated?.Invoke();
                return true;
            }
        }

        // Boþ bir slot bulup oraya eklemeyi dene
        InventorySlot emptySlot = FindEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.UpdateSlot(item, amount);
            OnInventoryUpdated?.Invoke();
            return true;
        }

        // Hiç yer yoksa
        Debug.Log("Envanterde yer yok!");
        return false;
    }

    // YENÝ FONKSÝYON: Envanterden eþya silmek için
    public void RemoveFromInventory(InventorySlot slotToRemove)
    {
        // Bu fonksiyon, biriktirilebilir eþyalardan sadece bir tane silmek yerine
        // þimdilik tüm slotu temizler. Bu, satýþ için yeterlidir.
        slotToRemove.ClearSlot();

        // Deðiþikliði UI'a ve diðer sistemlere bildir.
        OnInventoryUpdated?.Invoke();
    }

    private InventorySlot FindSlotForStacking(ItemData item)
    {
        return inventorySlots.FirstOrDefault(s => s.itemData == item && s.stackSize < item.maxStackSize);
    }

    private InventorySlot FindEmptySlot()
    {
        return inventorySlots.FirstOrDefault(s => s.itemData == null);
    }
}