// InventoryUI.cs
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // Inspector'dan atayacaðýmýz ana panel
    [SerializeField] private Transform slotGrid; // Slotlarýn oluþturulacaðý Grid Layout objesi
    [SerializeField] private GameObject slotPrefab; // Az önce oluþturduðumuz InventorySlot_UI prefab'ý

    private PlayerInventory playerInventory;
    private List<InventorySlot_UI_Controller> slotUIControllers = new List<InventorySlot_UI_Controller>();

    private void Start()
    {
        // Oyuncunun envanterini bul ve event'ine abone ol.
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        playerInventory.inventory.OnInventorySlotChanged += UpdateSlotUI;

        // Baþlangýçta envanteri kapalý tut.
        inventoryPanel.SetActive(false);
        InitializeInventoryUI();
    }

    // Arayüzü envanter boyutuna göre oluþturur.
    private void InitializeInventoryUI()
    {
        for (int i = 0; i < playerInventory.inventory.InventorySize; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotGrid);
            slotUIControllers.Add(newSlot.GetComponent<InventorySlot_UI_Controller>());
        }
        UpdateAllSlotsUI(); // Tüm slotlarý ilk kez güncelle
    }

    // Arayüzdeki TEK BÝR slotu günceller.
    private void UpdateSlotUI(InventorySlot changedSlot)
    {
        int index = playerInventory.inventory.InventorySlots.IndexOf(changedSlot);
        if (index >= 0 && index < slotUIControllers.Count)
        {
            slotUIControllers[index].UpdateSlot(changedSlot);
        }
    }

    // Arayüzdeki TÜM slotlarý günceller (envanter ilk açýldýðýnda vs.)
    private void UpdateAllSlotsUI()
    {
        for (int i = 0; i < slotUIControllers.Count; i++)
        {
            slotUIControllers[i].UpdateSlot(playerInventory.inventory.InventorySlots[i]);
        }
    }

    // Envanteri açýp kapatmak için bir fonksiyon
    void Update()
    {
        // 'I' tuþuna basýldýðýnda envanteri aç/kapat.
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);

            // Paneli her açtýðýmýzda güncel verileri gösterdiðinden emin olalým.
            if (inventoryPanel.activeSelf)
            {
                UpdateAllSlotsUI();
            }
        }
    }
}