// InventorySystem.cs
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] // Inspector'da g�r�lebilmesi i�in
public class InventorySystem
{
    // Envanterdeki t�m slotlar� tutan bir liste.
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => inventorySlots.Count;

    // Envanter her g�ncellendi�inde tetiklenecek olan event. Aray�z bu event'i dinleyecek.
    public event Action<InventorySlot> OnInventorySlotChanged;

    // Yap�c� Metot: Belirtilen boyutta bir envanter olu�turur.
    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    // Envantere e�ya eklemeye �al��an ana fonksiyon.
    public bool AddToInventory(ItemData item, int amount)
    {
        // Eklenecek e�yan�n, envanterde zaten var olan ve biriktirilebilen bir slota s���p s��mad���n� kontrol et.
        InventorySlot slot = FindSlotForStacking(item);
        if (slot != null)
        {
            slot.AddToStack(amount);
            OnInventorySlotChanged?.Invoke(slot);
            return true;
        }

        // E�er biriktirilemiyorsa veya mevcut slotlar doluysa, bo� bir slot bul.
        slot = FindEmptySlot();
        if (slot != null)
        {
            slot.itemData = item;
            slot.stackSize = amount;
            OnInventorySlotChanged?.Invoke(slot);
            return true;
        }

        // Envanterde hi� yer yoksa, ekleme i�leminin ba�ar�s�z oldu�unu belirt.
        return false;
    }

    // Biriktirilebilir bir e�ya i�in uygun slot arayan fonksiyon.
    private InventorySlot FindSlotForStacking(ItemData item)
    {
        // Envanterdeki her slotu kontrol et.
        foreach (InventorySlot slot in inventorySlots)
        {
            // E�er slot bo� de�ilse, ayn� e�yay� i�eriyorsa, biriktirilebilirse ve slot dolu de�ilse...
            if (slot.itemData != null &&
                slot.itemData == item &&
                item.isStackable &&
                slot.stackSize < item.maxStackSize)
            {
                return slot; // O slotu geri d�nd�r.
            }
        }
        return null; // Uygun slot bulunamad�.
    }

    // Tamamen bo� bir slot arayan fonksiyon.
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