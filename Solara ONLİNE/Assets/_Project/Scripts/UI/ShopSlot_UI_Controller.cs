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

    // D�kkan e�yas�n� g�stermek i�in
    public void SetupVendorSlot(ShopItem shopItem)
    {
        Debug.Log("--- SetupVendorSlot �a�r�ld�. ---");

        if (shopItem == null) { Debug.LogError("HATA: Gelen shopItem verisi BO� (NULL)!"); return; }

        ItemData itemData = shopItem.item;

        if (itemData == null) { Debug.LogError("HATA: shopItem i�indeki 'item' verisi BO� (NULL)!"); return; }

        heldItem = shopItem;

        if (itemIcon == null) { Debug.LogError("HATA: Inspector'da 'itemIcon' referans� atanmam��!"); return; }

        Debug.Log("Atanacak item: " + itemData.itemName + ", �konu var m�?: " + (itemData.icon != null));

        itemIcon.sprite = itemData.icon;
        itemIcon.enabled = true;
        stackCountText.enabled = false;
        button.interactable = true;

        Debug.Log("--- Slot ba�ar�yla kuruldu. ---");
    }

    // Oyuncu envanter e�yas�n� g�stermek i�in
    public void SetupPlayerSlot(InventorySlot inventorySlot)
    {
        // ... (bu k�s�m �imdilik ayn� kalabilir) ...
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