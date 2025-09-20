// InventorySlot.cs
using UnityEngine;
using System; // Serializable olmas� i�in gerekli

[Serializable] // Bu sat�r, bu s�n�f�n Unity Inspector'da g�r�n�r olmas�n� sa�lar.
public class InventorySlot
{
    public ItemData itemData; // Bu slotta hangi e�yan�n verisi var?
    public int stackSize;     // Bu slottaki e�yadan ka� tane var?

    // Yap�c� (Constructor) Metot: Yeni bir slot olu�turuldu�unda �a�r�l�r.
    public InventorySlot(ItemData item, int size)
    {
        itemData = item;
        stackSize = size;
    }

    // Bo� bir slot olu�turmak i�in yap�c� metot.
    public InventorySlot()
    {
        itemData = null;
        stackSize = 0;
    }

    // Bu slottaki e�ya miktar�n� art�rmak i�in bir fonksiyon.
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