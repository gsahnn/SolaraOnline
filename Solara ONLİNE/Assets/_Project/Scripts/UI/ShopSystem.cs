using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; // Button'larý kodda kullanmak için bu satýr gerekli!

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }

    [Header("Ana Paneller ve Konteynerlar")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform vendorItemContainer;
    [SerializeField] private Transform playerInventoryContainer;

    [Header("Prefab")]
    [SerializeField] private GameObject shopSlotPrefab;

    [Header("Butonlar ve Detaylar")]
    [SerializeField] private Button buyButton; // "Satýn Al" butonu
    [SerializeField] private Button sellButton; // "Sat" butonu
    [SerializeField] private Button closeButton; // "Kapat" butonu
    [SerializeField] private TextMeshProUGUI playerGoldText;

    private PlayerInventory playerInventory;
    private CharacterStats playerStats;
    private ShopSlotController selectedSlot;

    private List<GameObject> spawnedShopSlots = new List<GameObject>();
    private List<GameObject> spawnedPlayerSlots = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; }

        shopPanel.SetActive(false);
    }

    private void Start()
    {
        // --- BUTON ONCLICK ATAMALARI BURADA YAPILIYOR ---
        // Bu, Inspector'dan atamayý unutsak bile, kodun butonlara ne yapacaðýný
        // söylemesini garanti altýna alýr.

        if (buyButton != null)
            buyButton.onClick.AddListener(ConfirmPurchase);

        if (sellButton != null)
            sellButton.onClick.AddListener(ConfirmSale);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseShop);
        // ----------------------------------------------------
    }

    public void OpenShop(ShopData shopData, PlayerInventory inv)
    {
        playerInventory = inv;
        playerStats = inv.GetComponent<CharacterStats>();
        shopPanel.SetActive(true);

        playerInventory.inventory.OnInventoryUpdated += RefreshPlayerInventoryUI;

        RefreshVendorUI(shopData);
        RefreshPlayerInventoryUI();
        DeselectCurrent(); // Dükkan açýldýðýnda hiçbir þeyin seçili olmadýðýndan emin ol.
    }

    public void CloseShop()
    {
        if (playerInventory != null)
            playerInventory.inventory.OnInventoryUpdated -= RefreshPlayerInventoryUI;
        shopPanel.SetActive(false);
    }

    private void RefreshVendorUI(ShopData shopData)
    {
        foreach (var slot in spawnedShopSlots) Destroy(slot);
        spawnedShopSlots.Clear();

        foreach (var shopItem in shopData.itemsForSale)
        {
            var slotGO = Instantiate(shopSlotPrefab, vendorItemContainer);
            slotGO.GetComponent<ShopSlotController>().SetupVendorSlot(shopItem);
        }
    }

    private void RefreshPlayerInventoryUI()
    {
        foreach (var slot in spawnedPlayerSlots) Destroy(slot);
        spawnedPlayerSlots.Clear();

        if (playerInventory == null) return;
        foreach (var invSlot in playerInventory.inventory.InventorySlots)
        {
            var slotGO = Instantiate(shopSlotPrefab, playerInventoryContainer);
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
        if (selectedSlot != null) selectedSlot.Deselect();
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
}