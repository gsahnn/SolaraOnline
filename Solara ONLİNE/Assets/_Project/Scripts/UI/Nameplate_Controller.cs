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
        // Baþlangýçta can barýný her zaman gizle, Initialize karar verecek.
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
            // Evet, oyuncu. Sadece ismini göster, can barýný gizle.
            healthSlider.gameObject.SetActive(false);
            nameText.text = $"Lv. {stats.level} {stats.gameObject.name}";
            // Oyuncunun kendi statlarý deðiþtiðinde isminin güncellenmesi için de dinleyebiliriz.
            stats.OnStatsChanged += UpdatePlayerInfo;
            UpdatePlayerInfo(stats);
        }
        else // Hayýr, bu bir canavar.
        {
            healthSlider.gameObject.SetActive(true);
            stats.OnStatsChanged += UpdateMonsterInfo; // Artýk imzalar eþleþiyor.
            UpdateMonsterInfo(stats);
        }
    }

    // Canavarýn bilgilerini güncelleyen fonksiyon.
    private void UpdateMonsterInfo(CharacterStats stats)
    {
        if (stats == null) return;
        nameText.text = $"Lv. {stats.level} {stats.gameObject.name}";
        healthSlider.maxValue = stats.maxHealth;
        healthSlider.value = stats.currentHealth;
    }

    // Oyuncunun bilgilerini güncelleyen fonksiyon (can barý olmadan).
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