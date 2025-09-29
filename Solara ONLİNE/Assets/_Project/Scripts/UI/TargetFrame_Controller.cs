using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetFrame_Controller : MonoBehaviour
{
    public static TargetFrame_Controller Instance { get; private set; }

    [Header("UI Referanslar�")]
    [SerializeField] private GameObject targetFramePanel;
    [SerializeField] private TextMeshProUGUI targetNameText;
    [SerializeField] private Slider targetHealthSlider;

    [Header("D�nya Referanslar�")]
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

        // Emin olmak i�in, hala dinliyorsak abonelikten ��k
        if (currentTargetStats != null) currentTargetStats.OnStatsChanged -= UpdateHealthBar;
    }

    private void UpdateTargetFrame(Transform newTarget)
    {
        // �nceki hedefin �emberini yok et ve event aboneli�ini bitir.
        if (currentTargetCircleInstance != null) Destroy(currentTargetCircleInstance);
        if (currentTargetStats != null) currentTargetStats.OnStatsChanged -= UpdateHealthBar;

        if (newTarget != null && newTarget.TryGetComponent(out currentTargetStats))
        {
            targetFramePanel.SetActive(true);
            currentTargetStats.OnStatsChanged += UpdateHealthBar;
            UpdateHealthBar(currentTargetStats); // �lk �a�r�y� parametre ile yap.

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

    // --- D�ZELT�LM�� FONKS�YON ---
    // Bu fonksiyon art�k bir CharacterStats parametresi al�yor ve event'in imzas�n� kar��l�yor.
    private void UpdateHealthBar(CharacterStats stats)
    {
        // Hangi stat'lar� g�ncelleyece�imizi zaten 'currentTargetStats' de�i�keninde biliyoruz.
        if (currentTargetStats != null)
        {
            targetNameText.text = $"Lv. {currentTargetStats.level} {currentTargetStats.gameObject.name}";
            targetHealthSlider.maxValue = currentTargetStats.maxHealth;
            targetHealthSlider.value = currentTargetStats.currentHealth;
        }
    }
}