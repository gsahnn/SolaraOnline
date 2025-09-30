using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetFrame_UI : MonoBehaviour
{
    [Header("UI Elementleri")]
    [SerializeField] private GameObject targetFrameObject;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Image healthBarFill; // Slider yerine doðrudan Image kullanýyoruz.
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

    // Hedef deðiþtiðinde bu fonksiyon çaðrýlýr.
    private void OnTargetChanged(CharacterStats newTarget)
    {
        StopTrackingCurrentTarget(); // Önceki hedefi dinlemeyi býrak.

        if (newTarget == null)
        {
            targetFrameObject.SetActive(false);
            return;
        }

        currentTrackedStats = newTarget;
        currentTrackedStats.OnStatsChanged += UpdateHealthUI; // Yeni hedefin can deðiþikliklerini dinlemeye baþla.

        targetFrameObject.SetActive(true);
        UpdateAllUI(currentTrackedStats); // Tüm UI'ý ilk verilerle doldur.
    }

    // Sadece can deðiþtiðinde çaðrýlýr (daha performanslý).
    private void UpdateHealthUI(CharacterStats stats)
    {
        if (stats.maxHealth > 0)
        {
            healthBarFill.fillAmount = (float)stats.currentHealth / stats.maxHealth;
        }
    }

    // Sadece ilk hedef alýndýðýnda veya statlar sýfýrlandýðýnda çaðrýlýr.
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