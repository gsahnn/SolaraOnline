// ShopSlot_UI_Controller.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot_UI_Controller : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;

    private ShopItem currentShopItem;

    public void SetItem(ShopItem shopItem)
    {
        currentShopItem = shopItem;

        itemIcon.sprite = shopItem.item.icon;
        itemNameText.text = shopItem.item.itemName;
        itemPriceText.text = shopItem.price.ToString() + " Alt�n";
    }

    // Bu fonksiyon, Inspector'dan bu slotun kendisine atanm�� olan
    // bir Button bile�eninin OnClick() olay�na ba�lanacak.
    public void OnClick_BuyItem()
    {
        if (currentShopItem != null)
        {
            ShopSystem.Instance.AttemptToBuyItem(currentShopItem);
        }
    }
}