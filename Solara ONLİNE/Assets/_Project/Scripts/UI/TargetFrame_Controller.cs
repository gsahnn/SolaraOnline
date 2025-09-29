using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetFrame_Controller : MonoBehaviour
{
    public static TargetFrame_Controller Instance { get; private set; }

    [Header("UI Referanslarý")]
    [SerializeField] private GameObject targetFramePanel;
    [SerializeField] private TextMeshProUGUI targetNameText;
    [SerializeField] private Slider targetHealthSlider;

    [Header("Dünya Referanslarý")]
    [SerializeField] private GameObject targetCirclePrefab;

    private CharacterStats currentTargetStats;
    private GameObject currentTargetCircleInstance;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    public void Initialize(PlayerTargeting playerTargeting)
    {
        if (playerTargeting != null)
            playerTargeting.OnTargetSelected += UpdateTargetFrame;

        if (targetFramePanel != null)
            targetFramePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerTargeting playerTargeting = FindFirstObjectByType<PlayerTargeting>();
        if (playerTargeting != null) playerTargeting.OnTargetSelected -= UpdateTargetFrame;

        // Emin olmak için, hala dinliyorsak abonelikten çýk
        if (currentTargetStats != null) currentTargetStats.OnStatsChanged -= UpdateHealthBar;
    }

    private void UpdateTargetFrame(Transform newTarget)
    {
        // Önceki hedefin çemberini yok et ve event aboneliðini bitir.
        if (currentTargetCircleInstance != null) Destroy(currentTargetCircleInstance);
        if (currentTargetStats != null) currentTargetStats.OnStatsChanged -= UpdateHealthBar;

        if (newTarget != null && newTarget.TryGetComponent(out currentTargetStats))
        {
            targetFramePanel.SetActive(true);
            currentTargetStats.OnStatsChanged += UpdateHealthBar;
            UpdateHealthBar(currentTargetStats); // Ýlk çaðrýyý parametre ile yap.

            if (targetCirclePrefab != null)
            {
                currentTargetCircleInstance = Instantiate(targetCirclePrefab, newTarget.position, newTarget.rotation, newTarget);
            }
        }
        else
        {
            currentTargetStats = null;
            targetFramePanel.SetActive(false);
        }
    }

    // --- DÜZELTÝLMÝÞ FONKSÝYON ---
    // Bu fonksiyon artýk bir CharacterStats parametresi alýyor ve event'in imzasýný karþýlýyor.
    private void UpdateHealthBar(CharacterStats stats)
    {
        // Hangi stat'larý güncelleyeceðimizi zaten 'currentTargetStats' deðiþkeninde biliyoruz.
        if (currentTargetStats != null)
        {
            targetNameText.text = $"Lv. {currentTargetStats.level} {currentTargetStats.gameObject.name}";
            targetHealthSlider.maxValue = currentTargetStats.maxHealth;
            targetHealthSlider.value = currentTargetStats.currentHealth;
        }
    }
}