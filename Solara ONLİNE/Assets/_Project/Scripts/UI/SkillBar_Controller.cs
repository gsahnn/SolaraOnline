using System.Collections.Generic;
using UnityEngine;

public class SkillBar_Controller : MonoBehaviour
{
    public static SkillBar_Controller Instance { get; private set; }

    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    private SkillHolder playerSkillHolder;
    private List<SkillSlot_UI_Controller> slotUIControllers = new List<SkillSlot_UI_Controller>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }
    }

    public void Initialize(SkillHolder skillHolder)
    {
        if (playerSkillHolder != null) playerSkillHolder.OnSkillsUpdated -= UpdateSkillBarUI;

        playerSkillHolder = skillHolder;
        playerSkillHolder.OnSkillsUpdated += UpdateSkillBarUI;
        SetupSkillBar();
    }

    private void OnDestroy()
    {
        if (playerSkillHolder != null) playerSkillHolder.OnSkillsUpdated -= UpdateSkillBarUI;
    }

    private void SetupSkillBar()
    {
        foreach (Transform child in slotContainer) { Destroy(child.gameObject); }
        slotUIControllers.Clear();

        if (playerSkillHolder == null) return;

        for (int i = 0; i < playerSkillHolder.skills.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            slotUIControllers.Add(newSlot.GetComponent<SkillSlot_UI_Controller>());
        }
        UpdateSkillBarUI();
    }

    private void UpdateSkillBarUI()
    {
        if (playerSkillHolder == null) return;

        for (int i = 0; i < slotUIControllers.Count; i++)
        {
            if (i < playerSkillHolder.skills.Count)
            {
                // --- DÜZELTME BURADA: Fonksiyon adýný 'UpdateSlotUI' olarak güncelledik. ---
                slotUIControllers[i].UpdateSlotUI(playerSkillHolder.skills[i]);
            }
            else
            {
                // --- DÜZELTME BURADA: Fonksiyon adýný 'UpdateSlotUI' olarak güncelledik. ---
                slotUIControllers[i].UpdateSlotUI(null);
            }
        }
    }
}