using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetFrame_UI : MonoBehaviour
{
    [Header("UI Elementleri")]
    [SerializeField] private GameObject targetFrameObject;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Image healthBarFill; // Slider yerine do�rudan Image kullan�yoruz.
    [SerializeField] private TextMeshProUGUI raceText;
    [SerializeField] private Button closeButton;

    private CharacterStats currentTrackedStats;

    private void Start()
    {
        PlayerTargeting.OnTargetSelected += OnTargetChanged;
        closeButton.onClick.AddListener(OnCloseButtonPressed);
        targetFrameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerTargeting.OnTargetSelected -= OnTargetChanged;
        StopTrackingCurrentTarget();
    }

    // Hedef de�i�ti�inde bu fonksiyon �a�r�l�r.
    private void OnTargetChanged(CharacterStats newTarget)
    {
        StopTrackingCurrentTarget(); // �nceki hedefi dinlemeyi b�rak.

        if (newTarget == null)
        {
            targetFrameObject.SetActive(false);
            return;
        }

        currentTrackedStats = newTarget;
        currentTrackedStats.OnStatsChanged += UpdateHealthUI; // Yeni hedefin can de�i�ikliklerini dinlemeye ba�la.

        targetFrameObject.SetActive(true);
        UpdateAllUI(currentTrackedStats); // T�m UI'� ilk verilerle doldur.
    }

    // Sadece can de�i�ti�inde �a�r�l�r (daha performansl�).
    private void UpdateHealthUI(CharacterStats stats)
    {
        if (stats.maxHealth > 0)
        {
            healthBarFill.fillAmount = (float)stats.currentHealth / stats.maxHealth;
        }
    }

    // Sadece ilk hedef al�nd���nda veya statlar s�f�rland���nda �a�r�l�r.
    private void UpdateAllUI(CharacterStats stats)
    {
        infoText.text = $"Lv.{stats.level} ({stats.characterGrade}) {stats.characterName}";

        if (!string.IsNullOrEmpty(stats.raceName))
        {
            raceText.text = stats.raceName;
            raceText.gameObject.SetActive(true);
        }
        else
        {
            raceText.gameObject.SetActive(false);
        }

        UpdateHealthUI(stats);
    }

    private void OnCloseButtonPressed()
    {
        PlayerTargeting.Instance.ClearTarget();
    }

    private void StopTrackingCurrentTarget()
    {
        if (currentTrackedStats != null)
        {
            currentTrackedStats.OnStatsChanged -= UpdateHealthUI;
            currentTrackedStats = null;
        }
    }
}