// SkillSlot_UI_Controller.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot_UI_Controller : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText;

    // Bu slotu, belirli bir SkillSlot verisiyle g�nceller.
    public void UpdateSlot(SkillSlot skillSlot)
    {
        // Yetenek var m�?
        if (skillSlot != null && skillSlot.skillData != null)
        {
            skillIcon.enabled = true;
            skillIcon.sprite = skillSlot.skillData.icon;

            // Bekleme s�resinde mi?
            if (skillSlot.IsOnCooldown())
            {
                cooldownOverlay.enabled = true;
                cooldownText.enabled = true;

                // Dolum efektini ayarla (0 ile 1 aras�nda bir de�er)
                cooldownOverlay.fillAmount = skillSlot.cooldownTimer / skillSlot.skillData.cooldown;
                // Kalan s�reyi yazd�r (�rn: 3.2s -> "3", 0.8s -> "1")
                cooldownText.text = Mathf.CeilToInt(skillSlot.cooldownTimer).ToString();
            }
            else
            {
                // Bekleme s�resi bittiyse g�rselleri kapat.
                cooldownOverlay.enabled = false;
                cooldownText.enabled = false;
            }
        }
        else
        {
            // Slotta yetenek yoksa her �eyi gizle.
            skillIcon.enabled = false;
            cooldownOverlay.enabled = false;
            cooldownText.enabled = false;
        }
    }
}