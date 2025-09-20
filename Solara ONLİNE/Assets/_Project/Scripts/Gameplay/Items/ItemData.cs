// ItemData.cs
using UnityEngine;


[CreateAssetMenu(fileName = "New Generic Item", menuName = "Solara/Data/Items/Generic Item")]
public class ItemData : ScriptableObject
{
    [Header("Temel Bilgiler")] // Inspector'da ba�l�k olu�turur.
    public string itemName;
    public Sprite icon;

    [TextArea(4, 8)] // A��klama alan�n� daha b�y�k bir metin kutusu yapar.
    public string description;

    [Header("E�ya Ayarlar�")]
    public bool isStackable; // Bu e�ya �st �ste birikebilir mi? (�ksir evet, k�l�� hay�r)
    public int maxStackSize = 1; // E�er birikebiliyorsa, en fazla ka� tane olabilir?
    public int sellPrice = 1; //E�yan�n temel sat�� fiyat�.
}