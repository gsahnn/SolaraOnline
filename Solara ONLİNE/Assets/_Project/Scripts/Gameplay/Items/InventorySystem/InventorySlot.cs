// InventorySlot.cs
using UnityEngine;
using System; // Serializable olmasý için gerekli

[Serializable] // Bu satýr, bu sýnýfýn Unity Inspector'da görünür olmasýný saðlar.
public class InventorySlot
{
    public ItemData itemData; // Bu slotta hangi eþyanýn verisi var?
    public int stackSize;     // Bu slottaki eþyadan kaç tane var?

    // Yapýcý (Constructor) Metot: Yeni bir slot oluþturulduðunda çaðrýlýr.
    public InventorySlot(ItemData item, int size)
    {
        itemData = item;
        stackSize = size;
    }

    // Boþ bir slot oluþturmak için yapýcý metot.
    public InventorySlot()
    {
        itemData = null;
        stackSize = 0;
    }

    // Bu slottaki eþya miktarýný artýrmak için bir fonksiyon.
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }
    public void UpdateSlot(ItemData data, int amount)
    {
        itemData = data;
        stackSize = amount;
    }

    public void ClearSlot()
    {
        itemData = null;
        stackSize = 0;
    }
}