using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System; // Action i�in gerekli

[RequireComponent(typeof(Button))] // Bu objeye bir Button bile�eni eklenmesini zorunlu k�l.
public class Shop_PlayerSlot_Controller : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;

    private Button button;
    private InventorySlot currentInventorySlot;

    private void Awake()
    {
        // Bu objenin �zerindeki Button bile�enini bul.
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

            // E�er slotta bir e�ya varsa, butonu t�klanabilir yap.
            button.interactable = true;
        }
        else
        {
            // E�er slot bo�sa, her �eyi devre d��� b�rak.
            itemIcon.enabled = false;
            stackCountText.enabled = false;
            button.interactable = false;
        }
    }

    // YEN� FONKS�YON: Bu, ShopSystem'in bu butona t�kland���nda ne yapaca��n� s�ylemesini sa�lar.
    public void AddListener(Action<InventorySlot> onClickAction)
    {
        // �nceki t�m komutlar� temizle (hatalar� �nlemek i�in), sonra yenisini ekle.
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickAction(currentInventorySlot));
    }
}