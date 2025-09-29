using UnityEngine;
using TMPro;

public class Nameplate_Controller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guildNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI characterNameText;

    private Transform cameraTransform;
    private CharacterStats myStats;

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
        stats.OnStatsChanged += UpdateInfo;
        UpdateInfo(stats);
    }

    private void UpdateInfo(CharacterStats stats)
    {
        if (stats == null) return;

        if (!string.IsNullOrEmpty(stats.guildName))
        {
            guildNameText.gameObject.SetActive(true);
            guildNameText.text = $"[{stats.guildName}]";
        }
        else
        {
            guildNameText.gameObject.SetActive(false);
        }

        levelText.text = $"Lv. {stats.level}";
        characterNameText.text = stats.characterName;
    }

    private void OnDestroy()
    {
        if (myStats != null)
        {
            myStats.OnStatsChanged -= UpdateInfo;
        }
    }
}