using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System; // Action için gerekli

[RequireComponent(typeof(Button))] // Bu objeye bir Button bileþeni eklenmesini zorunlu kýl.
public class Shop_PlayerSlot_Controller : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;

    private Button button;
    private InventorySlot currentInventorySlot;

    private void Awake()
    {
        // Bu objenin üzerindeki Button bileþenini bul.
        button = GetComponent<Button>();
    }

    public void SetSlot(InventorySlot inventorySlot)
    {
        currentInventorySlot = inventorySlot;

        if (inventorySlot != null && inventorySlot.itemData != null)
        {
            itemIcon.sprite = inventorySlot.itemData.icon;
            itemIcon.enabled = true;

            if (inventorySlot.stackSize > 1)
            {
                stackCountText.text = inventorySlot.stackSize.ToString();
                stackCountText.enabled = true;
            }
            else
            {
                stackCountText.enabled = false;
            }

            // Eðer slotta bir eþya varsa, butonu týklanabilir yap.
            button.interactable = true;
        }
        else
        {
            // Eðer slot boþsa, her þeyi devre dýþý býrak.
            itemIcon.enabled = false;
            stackCountText.enabled = false;
            button.interactable = false;
        }
    }

    // YENÝ FONKSÝYON: Bu, ShopSystem'in bu butona týklandýðýnda ne yapacaðýný söylemesini saðlar.
    public void AddListener(Action<InventorySlot> onClickAction)
    {
        // Önceki tüm komutlarý temizle (hatalarý önlemek için), sonra yenisini ekle.
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickAction(currentInventorySlot));
    }
}