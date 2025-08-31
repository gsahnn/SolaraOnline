// ItemData.cs
using UnityEngine;

// Bu sat�r �ok �nemli. Unity'nin "Create Asset Menu" alt�nda bu script'i g�stermesini sa�lar.
// Art�k Proje penceresinde sa� t�klay�p "Create/Data/Items/Generic Item" yolunu izleyerek
// bu verilerden yeni dosyalar olu�turabilece�iz.
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
}