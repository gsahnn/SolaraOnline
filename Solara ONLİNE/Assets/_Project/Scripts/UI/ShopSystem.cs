using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }

    [Header("UI Referanslarý")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform shopSlotContainer;
    [SerializeField] private GameObject shopSlotPrefab;
    [SerializeField] private TextMeshProUGUI playerGoldText; // Oyuncunun parasýný gösterecek metin

    private PlayerInventory playerInventory;
    private CharacterStats playerStats;
    private ShopData currentShopData;
    private List<GameObject> spawnedSlots = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

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
        RefreshShopItems();
        UpdatePlayerGoldUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    private void RefreshShopItems()
    {
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        foreach (ShopItem shopItem in currentShopData.itemsForSale)
        {
            GameObject newSlot = Instantiate(shopSlotPrefab, shopSlotContainer);
            newSlot.GetComponent<ShopSlot_UI_Controller>().SetItem(shopItem);
            spawnedSlots.Add(newSlot);
        }
    }

    public void AttemptToBuyItem(ShopItem itemToBuy)
    {
        if (playerStats.gold >= itemToBuy.price)
        {
            if (playerInventory.inventory.AddToInventory(itemToBuy.item, 1))
            {
                playerStats.gold -= itemToBuy.price;
                Debug.Log(itemToBuy.item.itemName + " satýn alýndý!");
                UpdatePlayerGoldUI();
            }
            else
            {
                Debug.Log("Envanterde yer yok!");
            }
        }
        else
        {
            Debug.Log("Yeterli altýn yok!");
        }
    }

    private void UpdatePlayerGoldUI()
    {
        if (playerGoldText != null)
        {
            playerGoldText.text = "Altýn: " + playerStats.gold.ToString();
        }
    }
}