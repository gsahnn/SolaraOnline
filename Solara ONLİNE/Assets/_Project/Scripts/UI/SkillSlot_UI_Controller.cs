using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot_UI_Controller : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay; // Cooldown'u g�stermek i�in dairesel Image
    [SerializeField] private TextMeshProUGUI cooldownText;

    private SkillSlot currentSkillSlot;

    public void UpdateSlotUI(SkillSlot skillSlot)
    {
        currentSkillSlot = skillSlot;

        if (currentSkillSlot == null || currentSkillSlot.skillData == null)
        {
            // E�er slot bo�sa, her �eyi gizle.
            iconImage.enabled = false;
            cooldownOverlay.enabled = false;
            cooldownText.enabled = false;
            return;
        }

        // --- D�ZELTME 1: 'icon' yerine 'skillIcon' kullan�l�yor. ---
        iconImage.sprite = currentSkillSlot.skillData.skillIcon;
        iconImage.enabled = true;

        // --- D�ZELTME 2: 'cooldownTimer'a do�rudan eri�mek yerine public fonksiyon kullan�l�yor. ---
        if (currentSkillSlot.IsOnCooldown())
        {
            cooldownOverlay.enabled = true;
            cooldownText.enabled = true;

            // 'fillAmount' 0 ile 1 aras�nda bir de�er al�r. Bu fonksiyon tam olarak bunu yapar.
            cooldownOverlay.fillAmount = currentSkillSlot.GetCooldownProgress();
            // Kalan s�reyi g�stermek i�in (opsiyonel)
            // cooldownText.text = Mathf.Ceil(currentSkillSlot.cooldownTimer).ToString(); // Bu hala private oldu�u i�in �al��maz.
            // Bunun yerine, SkillSlot'a yeni bir public fonksiyon ekleyebilir veya UI'da sadece overlay'i g�sterebiliriz.
            // �imdilik sadece overlay'i g�sterelim, en temiz ��z�m.
            cooldownText.enabled = false; // Veya kalan s�reyi g�stermek i�in SkillSlot'a public bir getter ekle.
        }
        else
        {
            cooldownOverlay.enabled = false;
            cooldownText.enabled = false;
        }
    }
}