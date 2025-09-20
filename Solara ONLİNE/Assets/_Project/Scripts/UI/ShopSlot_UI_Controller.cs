using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))] // Bu objeye Button eklenmesini zorunlu k�lar.
public class ShopSlotController : MonoBehaviour
{
    [Header("UI Referanslar�")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;
    [SerializeField] private GameObject selectionOutline; // Se�im �er�evesi

    private Button button;
    private object heldItem;

    private void Awake()
    {
        // Script uyand���nda, kendi �zerindeki Button bile�enini bulur.
        button = GetComponent<Button>();
        // Ve bu butona t�kland���nda OnSlotClicked fonksiyonunu �a��rmas�n� s�yler.
        button.onClick.AddListener(OnSlotClicked);
        Deselect(); // Ba�lang��ta se�ili olmas�n.
    }

    // D�kkan e�yas�n� g�stermek i�in
    public void SetupVendorSlot(ShopItem shopItem)
    {
        heldItem = shopItem;
        var itemData = shopItem.item;

        itemIcon.sprite = itemData.icon;
        itemIcon.enabled = true;
        stackCountText.enabled = false;
        button.interactable = true;
    }

    // Oyuncu e�yas�n� g�stermek i�in
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
        // T�kland���nda, ana ShopSystem'e "Ben se�ildim!" diye haber verir.
        ShopSystem.Instance.OnSlotSelected(this);
    }

    public void Select() { if (selectionOutline != null) selectionOutline.SetActive(true); }
    public void Deselect() { if (selectionOutline != null) selectionOutline.SetActive(false); }
    public object GetHeldItem() { return heldItem; }
}