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

    // Bu event, envantere e�ya eklendi�inde veya envanterden e�ya silindi�inde tetiklenir.
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

        // Bo� bir slot bulup oraya eklemeyi dene
        InventorySlot emptySlot = FindEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.UpdateSlot(item, amount);
            OnInventoryUpdated?.Invoke();
            return true;
        }

        // Hi� yer yoksa
        Debug.Log("Envanterde yer yok!");
        return false;
    }

    // YEN� FONKS�YON: Envanterden e�ya silmek i�in
    public void RemoveFromInventory(InventorySlot slotToRemove)
    {
        // Bu fonksiyon, biriktirilebilir e�yalardan sadece bir tane silmek yerine
        // �imdilik t�m slotu temizler. Bu, sat�� i�in yeterlidir.
        slotToRemove.ClearSlot();

        // De�i�ikli�i UI'a ve di�er sistemlere bildir.
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