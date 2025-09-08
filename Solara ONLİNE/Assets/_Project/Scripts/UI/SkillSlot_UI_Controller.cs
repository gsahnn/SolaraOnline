// SkillSlot_UI_Controller.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot_UI_Controller : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText;

    // Bu slotu, belirli bir SkillSlot verisiyle günceller.
    public void UpdateSlot(SkillSlot skillSlot)
    {
        // Yetenek var mý?
        if (skillSlot != null && skillSlot.skillData != null)
        {
            skillIcon.enabled = true;
            skillIcon.sprite = skillSlot.skillData.icon;

            // Bekleme süresinde mi?
            if (skillSlot.IsOnCooldown())
            {
                cooldownOverlay.enabled = true;
                cooldownText.enabled = true;

                // Dolum efektini ayarla (0 ile 1 arasýnda bir deðer)
                cooldownOverlay.fillAmount = skillSlot.cooldownTimer / skillSlot.skillData.cooldown;
                // Kalan süreyi yazdýr (örn: 3.2s -> "3", 0.8s -> "1")
                cooldownText.text = Mathf.CeilToInt(skillSlot.cooldownTimer).ToString();
            }
            else
            {
                // Bekleme süresi bittiyse görselleri kapat.
                cooldownOverlay.enabled = false;
                cooldownText.enabled = false;
            }
        }
        else
        {
            // Slotta yetenek yoksa her þeyi gizle.
            skillIcon.enabled = false;
            cooldownOverlay.enabled = false;
            cooldownText.enabled = false;
        }
    }
}