using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot_UI_Controller : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay; // Cooldown'u göstermek için dairesel Image
    [SerializeField] private TextMeshProUGUI cooldownText;

    private SkillSlot currentSkillSlot;

    public void UpdateSlotUI(SkillSlot skillSlot)
    {
        currentSkillSlot = skillSlot;

        if (currentSkillSlot == null || currentSkillSlot.skillData == null)
        {
            // Eðer slot boþsa, her þeyi gizle.
            iconImage.enabled = false;
            cooldownOverlay.enabled = false;
            cooldownText.enabled = false;
            return;
        }

        // --- DÜZELTME 1: 'icon' yerine 'skillIcon' kullanýlýyor. ---
        iconImage.sprite = currentSkillSlot.skillData.skillIcon;
        iconImage.enabled = true;

        // --- DÜZELTME 2: 'cooldownTimer'a doðrudan eriþmek yerine public fonksiyon kullanýlýyor. ---
        if (currentSkillSlot.IsOnCooldown())
        {
            cooldownOverlay.enabled = true;
            cooldownText.enabled = true;

            // 'fillAmount' 0 ile 1 arasýnda bir deðer alýr. Bu fonksiyon tam olarak bunu yapar.
            cooldownOverlay.fillAmount = currentSkillSlot.GetCooldownProgress();
            // Kalan süreyi göstermek için (opsiyonel)
            // cooldownText.text = Mathf.Ceil(currentSkillSlot.cooldownTimer).ToString(); // Bu hala private olduðu için çalýþmaz.
            // Bunun yerine, SkillSlot'a yeni bir public fonksiyon ekleyebilir veya UI'da sadece overlay'i gösterebiliriz.
            // Þimdilik sadece overlay'i gösterelim, en temiz çözüm.
            cooldownText.enabled = false; // Veya kalan süreyi göstermek için SkillSlot'a public bir getter ekle.
        }
        else
        {
            cooldownOverlay.enabled = false;
            cooldownText.enabled = false;
        }
    }
}