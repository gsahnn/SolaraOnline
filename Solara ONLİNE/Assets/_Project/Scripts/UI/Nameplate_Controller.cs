using UnityEngine;
using TMPro;

public class Nameplate_Controller : MonoBehaviour
{
    [Header("UI Element References")]
    [SerializeField] private TextMeshProUGUI guildText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;

    private CharacterStats linkedStats;
    private Transform cameraTransform;

    private void Start()
    {
        if (Camera.main != null) { cameraTransform = Camera.main.transform; }
        else { Debug.LogError("Sahnede 'MainCamera' tag'ine sahip bir kamera bulunamadý!", this.gameObject); }
    }

    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                             cameraTransform.rotation * Vector3.up);
        }
    }

    public void Initialize(CharacterStats statsToLink)
    {
        linkedStats = statsToLink;
        linkedStats.OnStatsInitialized += UpdateUI;
        linkedStats.OnStatsChanged += UpdateUI;
    }

    private void UpdateUI(CharacterStats stats)
    {
        guildText.text = $"[{stats.guildName}]";
        levelText.text = $"Lv {stats.level}";
        nameText.text = stats.characterName;

        if (stats.rankDatabase != null)
        {
            // Veritabanýndan doðru rütbeyi alýyoruz.
            Rank rankData = stats.rankDatabase.GetRankByPoints(stats.alignment);

            // KURAL: Rütbe bulunduysa VE adý "Tarafsýz" deðilse göster.
            if (rankData != null && rankData.rankName != "Tarafsýz")
            {
                rankText.text = rankData.rankName;
                rankText.color = rankData.rankColor;
                rankText.gameObject.SetActive(true);
            }
            else
            {
                // Rütbe "Tarafsýz" ise veya bulunamadýysa gizle.
                rankText.gameObject.SetActive(false);
            }
        }
        else
        {
            rankText.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (linkedStats != null)
        {
            linkedStats.OnStatsInitialized -= UpdateUI;
            linkedStats.OnStatsChanged -= UpdateUI;
        }
    }
}