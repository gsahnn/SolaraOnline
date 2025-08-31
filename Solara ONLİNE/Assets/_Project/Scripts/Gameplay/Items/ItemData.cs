// ItemData.cs
using UnityEngine;

// Bu satýr çok önemli. Unity'nin "Create Asset Menu" altýnda bu script'i göstermesini saðlar.
// Artýk Proje penceresinde sað týklayýp "Create/Data/Items/Generic Item" yolunu izleyerek
// bu verilerden yeni dosyalar oluþturabileceðiz.
[CreateAssetMenu(fileName = "New Generic Item", menuName = "Solara/Data/Items/Generic Item")]
public class ItemData : ScriptableObject
{
    [Header("Temel Bilgiler")] // Inspector'da baþlýk oluþturur.
    public string itemName;
    public Sprite icon;

    [TextArea(4, 8)] // Açýklama alanýný daha büyük bir metin kutusu yapar.
    public string description;

    [Header("Eþya Ayarlarý")]
    public bool isStackable; // Bu eþya üst üste birikebilir mi? (Ýksir evet, kýlýç hayýr)
    public int maxStackSize = 1; // Eðer birikebiliyorsa, en fazla kaç tane olabilir?
}