using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }

    [Header("Ana Panel ve Konteynerlar")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform vendorItemContainer;
    [SerializeField] private Transform playerInventoryContainer;

    [Header("Prefab")]
    [SerializeField] private GameObject shopSlotPrefab; // Sadece TEK BÝR prefab referansý

    [Header("Butonlar ve Detaylar")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI playerGoldText;

    private PlayerInventory playerInventory;
    private CharacterStats playerStats;
    private ShopSlotController selectedSlot;

    private List<GameObject> spawnedSlots = new List<GameObject>(); // Artýk tek bir liste yeterli

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; }
        shopPanel.SetActive(false);
    }

    private void Start()
    {
        buyButton.onClick.AddListener(ConfirmPurchase);
        sellButton.onClick.AddListener(ConfirmSale);
        closeButton.onClick.AddListener(CloseShop);
    }

    public void OpenShop(ShopData shopData, PlayerInventory inv)
    {
        playerInventory = inv;
        playerStats = inv.GetComponent<CharacterStats>();
        shopPanel.SetActive(true);

        playerInventory.inventory.OnInventoryUpdated += RefreshPlayerInventoryUI;

        RefreshVendorUI(shopData);
        RefreshPlayerInventoryUI();
        DeselectCurrent();
    }

    public void CloseShop()
    {
        if (playerInventory != null)
            playerInventory.inventory.OnInventoryUpdated -= RefreshPlayerInventoryUI;
        shopPanel.SetActive(false);
    }

    private void RefreshVendorUI(ShopData shopData)
    {
        // Önce eski slotlarý temizle
        foreach (Transform child in vendorItemContainer) Destroy(child.gameObject);

        foreach (var shopItem in shopData.itemsForSale)
        {
            GameObject slotGO = Instantiate(shopSlotPrefab, vendorItemContainer);
            slotGO.GetComponent<ShopSlotController>().SetupVendorSlot(shopItem);
        }
    }

    private void RefreshPlayerInventoryUI()
    {
        // Önce eski slotlarý temizle
        foreach (Transform child in playerInventoryContainer) Destroy(child.gameObject);

        if (playerInventory == null) return;
        foreach (var invSlot in playerInventory.inventory.InventorySlots)
        {
            GameObject slotGO = Instantiate(shopSlotPrefab, playerInventoryContainer);
            slotGO.GetComponent<ShopSlotController>().SetupPlayerSlot(invSlot);
        }
        UpdatePlayerGoldUI();
    }

    public void OnSlotSelected(ShopSlotController slotController)
    {
        DeselectCurrent();
        selectedSlot = slotController;
        selectedSlot.Select();

        object item = selectedSlot.GetHeldItem();
        buyButton.interactable = item is ShopItem;
        sellButton.interactable = item is InventorySlot && ((InventorySlot)item).itemData != null;
    }

    private void DeselectCurrent()
    {
        // Deselect iþlemi için tüm slotlarý gezmemiz gerekiyor.
        foreach (Transform child in vendorItemContainer) child.GetComponent<ShopSlotController>()?.Deselect();
        foreach (Transform child in playerInventoryContainer) child.GetComponent<ShopSlotController>()?.Deselect();

        selectedSlot = null;
        buyButton.interactable = false;
        sellButton.interactable = false;
    }

    private void ConfirmPurchase()
    {
        if (selectedSlot == null || !(selectedSlot.GetHeldItem() is ShopItem)) return;
        ShopItem itemToBuy = (ShopItem)selectedSlot.GetHeldItem();
        if (playerStats.gold >= itemToBuy.price)
        {
            if (playerInventory.inventory.AddToInventory(itemToBuy.item, 1))
            {
                playerStats.gold -= itemToBuy.price;
            }
        }
    }

    private void ConfirmSale()
    {
        if (selectedSlot == null || !(selectedSlot.GetHeldItem() is InventorySlot)) return;
        InventorySlot itemToSell = (InventorySlot)selectedSlot.GetHeldItem();
        if (itemToSell.itemData != null)
        {
            playerStats.gold += itemToSell.itemData.sellPrice;
            playerInventory.inventory.RemoveFromInventory(itemToSell);
            DeselectCurrent();
        }
    }

    private void UpdatePlayerGoldUI()
    {
        if (playerGoldText != null && playerStats != null)
        {
            playerGoldText.text = playerStats.gold.ToString();
        }
    }

    public bool IsOpen()
    {
        
        return shopPanel != null && shopPanel.activeSelf;
    }
}