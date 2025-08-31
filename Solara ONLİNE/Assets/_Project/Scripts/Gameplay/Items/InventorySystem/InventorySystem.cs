// InventorySystem.cs
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] // Inspector'da görülebilmesi için
public class InventorySystem
{
    // Envanterdeki tüm slotlarý tutan bir liste.
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => inventorySlots.Count;

    // Envanter her güncellendiðinde tetiklenecek olan event. Arayüz bu event'i dinleyecek.
    public event Action<InventorySlot> OnInventorySlotChanged;

    // Yapýcý Metot: Belirtilen boyutta bir envanter oluþturur.
    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    // Envantere eþya eklemeye çalýþan ana fonksiyon.
    public bool AddToInventory(ItemData item, int amount)
    {
        // Eklenecek eþyanýn, envanterde zaten var olan ve biriktirilebilen bir slota sýðýp sýðmadýðýný kontrol et.
        InventorySlot slot = FindSlotForStacking(item);
        if (slot != null)
        {
            slot.AddToStack(amount);
            OnInventorySlotChanged?.Invoke(slot);
            return true;
        }

        // Eðer biriktirilemiyorsa veya mevcut slotlar doluysa, boþ bir slot bul.
        slot = FindEmptySlot();
        if (slot != null)
        {
            slot.itemData = item;
            slot.stackSize = amount;
            OnInventorySlotChanged?.Invoke(slot);
            return true;
        }

        // Envanterde hiç yer yoksa, ekleme iþleminin baþarýsýz olduðunu belirt.
        return false;
    }

    // Biriktirilebilir bir eþya için uygun slot arayan fonksiyon.
    private InventorySlot FindSlotForStacking(ItemData item)
    {
        // Envanterdeki her slotu kontrol et.
        foreach (InventorySlot slot in inventorySlots)
        {
            // Eðer slot boþ deðilse, ayný eþyayý içeriyorsa, biriktirilebilirse ve slot dolu deðilse...
            if (slot.itemData != null &&
                slot.itemData == item &&
                item.isStackable &&
                slot.stackSize < item.maxStackSize)
            {
                return slot; // O slotu geri döndür.
            }
        }
        return null; // Uygun slot bulunamadý.
    }

    // Tamamen boþ bir slot arayan fonksiyon.
    private InventorySlot FindEmptySlot()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemData == null)
            {
                return slot;
            }
        }
        return null;
    }
}