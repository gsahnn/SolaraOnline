// InventoryUI.cs
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // Inspector'dan atayaca��m�z ana panel
    [SerializeField] private Transform slotGrid; // Slotlar�n olu�turulaca�� Grid Layout objesi
    [SerializeField] private GameObject slotPrefab; // Az �nce olu�turdu�umuz InventorySlot_UI prefab'�

    private PlayerInventory playerInventory;
    private List<InventorySlot_UI_Controller> slotUIControllers = new List<InventorySlot_UI_Controller>();

    private void Start()
    {
        // Oyuncunun envanterini bul ve event'ine abone ol.
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        playerInventory.inventory.OnInventorySlotChanged += UpdateSlotUI;

        // Ba�lang��ta envanteri kapal� tut.
        inventoryPanel.SetActive(false);
        InitializeInventoryUI();
    }

    // Aray�z� envanter boyutuna g�re olu�turur.
    private void InitializeInventoryUI()
    {
        for (int i = 0; i < playerInventory.inventory.InventorySize; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotGrid);
            slotUIControllers.Add(newSlot.GetComponent<InventorySlot_UI_Controller>());
        }
        UpdateAllSlotsUI(); // T�m slotlar� ilk kez g�ncelle
    }

    // Aray�zdeki TEK B�R slotu g�nceller.
    private void UpdateSlotUI(InventorySlot changedSlot)
    {
        int index = playerInventory.inventory.InventorySlots.IndexOf(changedSlot);
        if (index >= 0 && index < slotUIControllers.Count)
        {
            slotUIControllers[index].UpdateSlot(changedSlot);
        }
    }

    // Aray�zdeki T�M slotlar� g�nceller (envanter ilk a��ld���nda vs.)
    private void UpdateAllSlotsUI()
    {
        for (int i = 0; i < slotUIControllers.Count; i++)
        {
            slotUIControllers[i].UpdateSlot(playerInventory.inventory.InventorySlots[i]);
        }
    }

    // Envanteri a��p kapatmak i�in bir fonksiyon
    void Update()
    {
        // 'I' tu�una bas�ld���nda envanteri a�/kapat.
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);

            // Paneli her a�t���m�zda g�ncel verileri g�sterdi�inden emin olal�m.
            if (inventoryPanel.activeSelf)
            {
                UpdateAllSlotsUI();
            }
        }
    }
}