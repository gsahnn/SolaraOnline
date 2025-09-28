using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetFrame_Controller : MonoBehaviour
{
    public static TargetFrame_Controller Instance { get; private set; }

    [SerializeField] private GameObject targetFramePanel;
    [SerializeField] private TextMeshProUGUI targetNameText;
    [SerializeField] private Slider targetHealthSlider;

    private CharacterStats currentTargetStats;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    public void Initialize(PlayerTargeting playerTargeting)
    {
        playerTargeting.OnTargetSelected += UpdateTargetFrame;
        targetFramePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        // Oyuncu yok edildiðinde event aboneliðini sonlandýrmak önemlidir.
        PlayerTargeting playerTargeting = FindFirstObjectByType<PlayerTargeting>();
        if (playerTargeting != null)
        {
            playerTargeting.OnTargetSelected -= UpdateTargetFrame;
        }
    }

    private void UpdateTargetFrame(Transform newTarget)
    {
        // Önceki hedefin event'inden aboneliði iptal et.
        if (currentTargetStats != null)
            currentTargetStats.OnStatsChanged -= UpdateHealthBar;

        if (newTarget != null && newTarget.TryGetComponent(out currentTargetStats))
        {
            targetFramePanel.SetActive(true);
            currentTargetStats.OnStatsChanged += UpdateHealthBar;
            UpdateHealthBar(); // Ýlk deðerleri ata.
        }
        else
        {
            currentTargetStats = null;
            targetFramePanel.SetActive(false);
        }
    }

    private void UpdateHealthBar()
    {
        if (currentTargetStats != null)
        {
            targetNameText.text = currentTargetStats.gameObject.name;
            targetHealthSlider.maxValue = currentTargetStats.maxHealth;
            targetHealthSlider.value = currentTargetStats.currentHealth;
        }
    }
}