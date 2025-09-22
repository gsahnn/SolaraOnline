using UnityEngine;
using TMPro;

public class WorldUI_Controller : MonoBehaviour
{
    public static WorldUI_Controller Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI regionNameText;
    [SerializeField] private Animator regionNameAnimator;
    [SerializeField] private string animationTriggerName = "Show";

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    private void Start()
    {
      //  if (regionNameText != null) regionNameText.gameObject.SetActive(false);
        Debug.Log("<color=cyan>WORLD UI:</color>+ newRegion.regionName) ");
    }
    
    public void ShowRegionName(RegionData newRegion)
    {
        if (newRegion == null || regionNameText == null) return;

        regionNameText.text = newRegion.regionName;

        if (regionNameAnimator != null)
        {
            regionNameAnimator.SetTrigger(animationTriggerName);
        }
    }
}