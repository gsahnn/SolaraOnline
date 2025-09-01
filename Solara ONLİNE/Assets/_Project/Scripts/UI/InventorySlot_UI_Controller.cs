// InventorySlot_UI_Controller.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro k�t�phanesi

public class InventorySlot_UI_Controller : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;

    // Bu slotun g�rselini, ald��� verilere g�re g�nceller.
    public void UpdateSlot(InventorySlot slot)
    {
        if (slot.itemData != null)
        {
            // Slot doluysa
            itemIcon.sprite = slot.itemData.icon;
            itemIcon.enabled = true; // �konu g�r�n�r yap

            // E�er e�ya biriktirilebiliyorsa ve 1'den fazlaysa, say�y� g�ster.
            if (slot.stackSize > 1)
            {
                stackCountText.text = slot.stackSize.ToString();
                stackCountText.enabled = true;
            }
            else // De�ilse, say�y� gizle.
            {
                stackCountText.enabled = false;
            }
        }
        else
        {
            // Slot bo�sa, her �eyi temizle ve gizle.
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            stackCountText.enabled = false;
        }
    }
}