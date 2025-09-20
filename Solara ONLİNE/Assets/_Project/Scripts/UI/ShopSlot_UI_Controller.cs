using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))] // Bu objeye Button eklenmesini zorunlu kýlar.
public class ShopSlotController : MonoBehaviour
{
    [Header("UI Referanslarý")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;
    [SerializeField] private GameObject selectionOutline; // Seçim çerçevesi

    private Button button;
    private object heldItem;

    private void Awake()
    {
        // Script uyandýðýnda, kendi üzerindeki Button bileþenini bulur.
        button = GetComponent<Button>();
        // Ve bu butona týklandýðýnda OnSlotClicked fonksiyonunu çaðýrmasýný söyler.
        button.onClick.AddListener(OnSlotClicked);
        Deselect(); // Baþlangýçta seçili olmasýn.
    }

    // Dükkan eþyasýný göstermek için
    public void SetupVendorSlot(ShopItem shopItem)
    {
        heldItem = shopItem;
        var itemData = shopItem.item;

        itemIcon.sprite = itemData.icon;
        itemIcon.enabled = true;
        stackCountText.enabled = false;
        button.interactable = true;
    }

    // Oyuncu eþyasýný göstermek için
    public void SetupPlayerSlot(InventorySlot inventorySlot)
    {
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
        // Týklandýðýnda, ana ShopSystem'e "Ben seçildim!" diye haber verir.
        ShopSystem.Instance.OnSlotSelected(this);
    }

    public void Select() { if (selectionOutline != null) selectionOutline.SetActive(true); }
    public void Deselect() { if (selectionOutline != null) selectionOutline.SetActive(false); }
    public object GetHeldItem() { return heldItem; }
}