// ShopSystem.cs
using UnityEngine;
using System.Collections.Generic;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform shopSlotContainer;
    [SerializeField] private GameObject shopSlotPrefab;

    private PlayerInventory playerInventory;
    private ShopData currentShopData;

    private void Awake() { Instance = this; }
    private void Start() { shopPanel.SetActive(false); }

    public void OpenShop(ShopData shopData, PlayerInventory inv)
    {
        playerInventory = inv;
        currentShopData = shopData;
        shopPanel.SetActive(true);
        RefreshShopItems();
    }

    private void RefreshShopItems()
    {
        // ... (Dükkan slotlarýný oluþturup dolduran kod) ...
    }

    public void BuyItem(ItemData item)
    {
        // ... (Satýn alma mantýðý) ...
    }

    public void SellItem(InventorySlot slot)
    {
        // ... (Satma mantýðý) ...
    }
}