using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }

    [Header("UI Referanslar� - Genel")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI playerGoldText;

    [Header("UI Referanslar� - D�kkan B�l�m�")]
    [SerializeField] private Transform shopSlotContainer;
    [SerializeField] private GameObject shopSlotPrefab;

    [Header("UI Referanslar� - Oyuncu Envanteri B�l�m�")]
    [SerializeField] private Transform playerInventoryContainer;
    [SerializeField] private GameObject playerInventorySlotPrefab; // Ayr� bir prefab kullanmak daha temiz.

    private PlayerInventory playerInventory;
    private CharacterStats playerStats;
    private ShopData currentShopData;

    private List<GameObject> spawnedShopSlots = new List<GameObject>();
    private List<GameObject> spawnedPlayerSlots = new List<GameObject>();

    private void Awake() { Instance = this; }

    private void Start()
    {
        shopPanel.SetActive(false);
    }

    public void OpenShop(ShopData shopData, PlayerInventory inv)
    {
        playerInventory = inv;
        playerStats = inv.GetComponent<CharacterStats>();
        currentShopData = shopData;

        shopPanel.SetActive(true);

        // Oyuncunun envanterindeki de�i�iklikleri dinlemeye ba�la.
        playerInventory.inventory.OnInventoryUpdated += RefreshPlayerInventory;

        RefreshShopItems();
        RefreshPlayerInventory();
        UpdatePlayerGoldUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        // Dinlemeyi b�rak ki gereksiz yere �al��mas�n.
        if (playerInventory != null)
            playerInventory.inventory.OnInventoryUpdated -= RefreshPlayerInventory;
    }

    private void RefreshShopItems()
    {
        foreach (var slot in spawnedShopSlots) Destroy(slot);
        spawnedShopSlots.Clear();

        foreach (ShopItem shopItem in currentShopData.itemsForSale)
        {
            GameObject newSlot = Instantiate(shopSlotPrefab, shopSlotContainer);
            newSlot.GetComponent<ShopSlot_UI_Controller>().SetItem(shopItem);
            spawnedShopSlots.Add(newSlot);
        }
    }

    private void RefreshPlayerInventory()
    {
        foreach (var slot in spawnedPlayerSlots) Destroy(slot);
        spawnedPlayerSlots.Clear();

        foreach (InventorySlot invSlot in playerInventory.inventory.InventorySlots)
        {
            GameObject newSlotGO = Instantiate(playerInventorySlotPrefab, playerInventoryContainer);
            var controller = newSlotGO.GetComponent<Shop_PlayerSlot_Controller>();
            controller.SetSlot(invSlot);
            controller.AddListener(AttemptToSellItem);

            spawnedPlayerSlots.Add(newSlotGO);
        }
        UpdatePlayerGoldUI();
    }

    public void AttemptToBuyItem(ShopItem itemToBuy)
    {
        if (playerStats.gold >= itemToBuy.price)
        {
            if (playerInventory.inventory.AddToInventory(itemToBuy.item, 1))
            {
                playerStats.gold -= itemToBuy.price;
            }
        }
    }

    // YEN� FONKS�YON: E�ya Satma
    public void AttemptToSellItem(InventorySlot itemToSell)
    {
        if (itemToSell.itemData != null)
        {
            // Oyuncunun paras�na e�yan�n sat�� fiyat�n� ekle.
            playerStats.gold += itemToSell.itemData.sellPrice;

            // E�yay� envanterden kald�r.
            playerInventory.inventory.RemoveFromInventory(itemToSell);

            // Sat�� sonras� hem envanteri hem de alt�n miktar�n� an�nda g�ncelle.
            // (Event'ler bunu otomatik yapacak)
        }
    }

    private void UpdatePlayerGoldUI()
    {
        if (playerGoldText != null)
        {
            playerGoldText.text = "Alt�n: " + playerStats.gold.ToString();
        }
    }
}