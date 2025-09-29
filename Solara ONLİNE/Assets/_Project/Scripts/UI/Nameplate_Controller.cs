using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Nameplate_Controller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Slider healthSlider;

    private Transform cameraTransform;
    private CharacterStats myStats;

    private void Awake()
    {
        // Ba�lang��ta can bar�n� her zaman gizle, Initialize karar verecek.
        healthSlider.gameObject.SetActive(false);
    }

    private void Start()
    {
        cameraTransform = Camera.main?.transform;
    }

    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                             cameraTransform.rotation * Vector3.up);
        }
    }

    public void Initialize(CharacterStats stats)
    {
        myStats = stats;

        if (stats.GetComponent<PlayerController>() != null) // Bu bir oyuncu mu?
        {
            // Evet, oyuncu. Sadece ismini g�ster, can bar�n� gizle.
            healthSlider.gameObject.SetActive(false);
            nameText.text = $"Lv. {stats.level} {stats.gameObject.name}";
            // Oyuncunun kendi statlar� de�i�ti�inde isminin g�ncellenmesi i�in de dinleyebiliriz.
            stats.OnStatsChanged += UpdatePlayerInfo;
            UpdatePlayerInfo(stats);
        }
        else // Hay�r, bu bir canavar.
        {
            healthSlider.gameObject.SetActive(true);
            stats.OnStatsChanged += UpdateMonsterInfo; // Art�k imzalar e�le�iyor.
            UpdateMonsterInfo(stats);
        }
    }

    // Canavar�n bilgilerini g�ncelleyen fonksiyon.
    private void UpdateMonsterInfo(CharacterStats stats)
    {
        if (stats == null) return;
        nameText.text = $"Lv. {stats.level} {stats.gameObject.name}";
        healthSlider.maxValue = stats.maxHealth;
        healthSlider.value = stats.currentHealth;
    }

    // Oyuncunun bilgilerini g�ncelleyen fonksiyon (can bar� olmadan).
    private void UpdatePlayerInfo(CharacterStats stats)
    {
        if (stats == null) return;
        nameText.text = $"Lv. {stats.level} {stats.gameObject.name}";
    }

    private void OnDestroy()
    {
        if (myStats != null)
        {
            myStats.OnStatsChanged -= UpdateMonsterInfo;
            myStats.OnStatsChanged -= UpdatePlayerInfo;
        }
    }
}