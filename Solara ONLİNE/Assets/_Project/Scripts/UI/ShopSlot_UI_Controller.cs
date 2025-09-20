using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ShopSlotController : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;
    [SerializeField] private GameObject selectionOutline;

    private Button button;
    private object heldItem;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSlotClicked);
        Deselect();
    }

    // Dükkan eþyasýný göstermek için
    public void SetupVendorSlot(ShopItem shopItem)
    {
        Debug.Log("--- SetupVendorSlot çaðrýldý. ---");

        if (shopItem == null) { Debug.LogError("HATA: Gelen shopItem verisi BOÞ (NULL)!"); return; }

        ItemData itemData = shopItem.item;

        if (itemData == null) { Debug.LogError("HATA: shopItem içindeki 'item' verisi BOÞ (NULL)!"); return; }

        heldItem = shopItem;

        if (itemIcon == null) { Debug.LogError("HATA: Inspector'da 'itemIcon' referansý atanmamýþ!"); return; }

        Debug.Log("Atanacak item: " + itemData.itemName + ", Ýkonu var mý?: " + (itemData.icon != null));

        itemIcon.sprite = itemData.icon;
        itemIcon.enabled = true;
        stackCountText.enabled = false;
        button.interactable = true;

        Debug.Log("--- Slot baþarýyla kuruldu. ---");
    }

    // Oyuncu envanter eþyasýný göstermek için
    public void SetupPlayerSlot(InventorySlot inventorySlot)
    {
        // ... (bu kýsým þimdilik ayný kalabilir) ...
        heldItem = inventorySlot;
        if (inventorySlot != null && inventorySlot.itemData != null)
        {
            itemIcon.sprite = inventorySlot.itemData.icon;
            itemIcon.enabled = true;
            stackCountText.enabled = inventorySlot.stackSize > 1;
            stackCountText.text = inventorySlot.stackSize.ToString();
            button.interactable = true;
        }
        else
        {
            itemIcon.enabled = false;
            stackCountText.enabled = false;
            button.interactable = false;
        }
    }

    private void OnSlotClicked()
    {
        ShopSystem.Instance.OnSlotSelected(this);
    }

    public void Select() { if (selectionOutline != null) selectionOutline.SetActive(true); }
    public void Deselect() { if (selectionOutline != null) selectionOutline.SetActive(false); }
    public object GetHeldItem() { return heldItem; }
}