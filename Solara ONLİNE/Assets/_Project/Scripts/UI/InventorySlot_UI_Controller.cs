// InventorySlot_UI_Controller.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kütüphanesi

public class InventorySlot_UI_Controller : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;

    // Bu slotun görselini, aldýðý verilere göre günceller.
    public void UpdateSlot(InventorySlot slot)
    {
        if (slot.itemData != null)
        {
            // Slot doluysa
            itemIcon.sprite = slot.itemData.icon;
            itemIcon.enabled = true; // Ýkonu görünür yap

            // Eðer eþya biriktirilebiliyorsa ve 1'den fazlaysa, sayýyý göster.
            if (slot.stackSize > 1)
            {
                stackCountText.text = slot.stackSize.ToString();
                stackCountText.enabled = true;
            }
            else // Deðilse, sayýyý gizle.
            {
                stackCountText.enabled = false;
            }
        }
        else
        {
            // Slot boþsa, her þeyi temizle ve gizle.
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            stackCountText.enabled = false;
        }
    }
}