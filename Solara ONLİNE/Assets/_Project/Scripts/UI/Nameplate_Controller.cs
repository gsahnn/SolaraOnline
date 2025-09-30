using UnityEngine;
using TMPro;

public class Nameplate_Controller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guildNameText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI characterNameText;

    [Header("Veritaban�")]
    [SerializeField] private RankDatabase rankDatabase;

    private const string NeutralRankName = "Tarafs�z";

    private Transform cameraTransform;
    private CharacterStats myStats;

    private void Awake()
    {
        if (guildNameText == null || rankText == null || levelText == null || characterNameText == null)
            Debug.LogError($"{nameof(Nameplate_Controller)}: UI referanslar� atanmad�!");

        if (rankDatabase == null)
            Debug.LogError($"{nameof(Nameplate_Controller)}: RankDatabase referans� atanmad�!");
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
        if (myStats != null)
            myStats.OnStatsChanged -= UpdateInfo;

        myStats = stats;
        myStats.OnStatsChanged += UpdateInfo;
        UpdateInfo(myStats);
    }

    private void UpdateInfo(CharacterStats stats)
    {
        if (stats == null) return;

        // Lonca
        if (!string.IsNullOrEmpty(stats.guildName))
        {
            guildNameText.text = stats.guildName;
            guildNameText.gameObject.SetActive(true);
        }
        else
        {
            guildNameText.gameObject.SetActive(false);
        }

        // Seviye ve �sim
        levelText.text = $"Lv. {stats.level}";
        characterNameText.text = stats.characterName;

        // --- YEN� DERECE MANTI�I ---
        RankData currentRank = rankDatabase.GetRankForAlignment(stats.alignment);
        if (currentRank != null && currentRank.rankName != NeutralRankName)
        {
            rankText.gameObject.SetActive(true);
            rankText.text = currentRank.rankName;
            rankText.color = currentRank.rankColor;
        }
        else // E�er r�tbe "Tarafs�z" ise veya bulunamad�ysa, unvan� gizle.
        {
            rankText.gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        if (myStats != null)
        {
            myStats.OnStatsChanged -= UpdateInfo;
        }
    }
}