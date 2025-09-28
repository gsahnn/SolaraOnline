using UnityEngine;
using System.Collections.Generic; // List için gerekli

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Veri Dosyalarý")]
    [SerializeField] private ComboData normalCombo_5; // 5'li kombo
    [SerializeField] private ComboData specialCombo_6; // 6'lý kombo (Beceri Seviye 1)
    [SerializeField] private ComboData specialCombo_7; // 7'li kombo (Beceri Seviye 2)

    [Header("Beceri Durumu")]
    // Bu deðerler, karakterin verileriyle birlikte kaydedilip yüklenecek.
    [Tooltip("Kombo becerisinin mevcut seviyesi (0 = öðrenilmemiþ, 1 = seviye 1, 2 = seviye 2)")]
    public int comboSkillLevel = 0;

    [Tooltip("Oyuncu beceriyi açtý mý?")]
    public bool isComboSkillActive = false;

    // Diðer script'ler
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    // Ýç sistem deðiþkenleri
    private ComboData currentCombo;
    private int comboCounter = 0;
    private float lastAttackTime = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        myStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        // Oyuna baþlarken, mevcut beceri durumuna göre doðru komboyu seç.
        UpdateCurrentCombo();
    }

    private void Update()
    {
        if (comboCounter > 0 && currentCombo != null && comboCounter < currentCombo.attacks.Count)
        {
            if (Time.time - lastAttackTime > currentCombo.attacks[comboCounter - 1].comboTimingWindow)
            {
                ResetCombo();
            }
        }
    }

    // Bu fonksiyon PlayerController tarafýndan çaðrýlýr.
    public void HandleAttackInput()
    {
        StartAttack();
    }

    private void StartAttack()
    {
        if (playerTargeting.currentTarget != null)
            transform.LookAt(playerTargeting.currentTarget.position);

        lastAttackTime = Time.time;

        // Mevcut kombo zincirinin bir sonraki adýmýný ateþle.
        string triggerToFire = currentCombo.attacks[comboCounter].triggerName;
        animator.SetTrigger(triggerToFire);

        comboCounter++;

        if (comboCounter >= currentCombo.attacks.Count)
        {
            ResetCombo();
        }
    }

    public void ResetCombo()
    {
        if (comboCounter == 0) return;

        // Hangi kombo setini kullandýðýmýzý bilmediðimiz için en güvenli yol,
        // olasý tüm trigger'larý sýfýrlamaktýr.
        for (int i = 1; i <= 7; i++)
        {
            animator.ResetTrigger("Attack" + i);
        }
        comboCounter = 0;
    }

    // Bu, kombo zinciri kýrýldýðýnda Animator Event'i ile çaðrýlýr.
    public void AnimationEvent_ResetCombo()
    {
        ResetCombo();
    }

    // Bu, her saldýrý animasyonunun vuruþ anýnda Animator Event'i ile çaðrýlýr.
    public void AnimationEvent_DealDamage()
    {
        // ... (Hasar verme mantýðý ayný, çarpan olmadýðý için basit) ...
    }

    // --- YENÝ VE ÖNEMLÝ FONKSÝYONLAR ---

    // Bu fonksiyon, oyuncu "Beceri Kitabý" okuduðunda çaðrýlýr.
    public void UpgradeComboSkill()
    {
        if (comboSkillLevel < 2) // Maksimum seviye 2
        {
            comboSkillLevel++;
            Debug.Log("Kombo Becerisi Seviye " + comboSkillLevel + " oldu!");
            // Becerinin durumu deðiþtiði için, mevcut komboyu güncelle.
            UpdateCurrentCombo();
        }
    }

    // Bu fonksiyon, oyuncu beceriyi UI'dan açýp kapattýðýnda çaðrýlýr.
    public void ToggleComboSkill()
    {
        // Eðer beceri hiç öðrenilmemiþse, açýp kapatýlamaz.
        if (comboSkillLevel == 0)
        {
            Debug.Log("Kombo Becerisi henüz öðrenilmemiþ!");
            return;
        }

        isComboSkillActive = !isComboSkillActive;
        Debug.Log("Kombo Becerisi " + (isComboSkillActive ? "AÇILDI" : "KAPATILDI"));
        UpdateCurrentCombo();
    }

    // Bu fonksiyon, hangi kombo setinin kullanýlacaðýna karar veren ana mantýktýr.
    private void UpdateCurrentCombo()
    {
        if (isComboSkillActive) // Beceri AÇIK ise...
        {
            if (comboSkillLevel == 1)
            {
                currentCombo = specialCombo_6; // 6'lý komboyu kullan.
            }
            else if (comboSkillLevel == 2)
            {
                currentCombo = specialCombo_7; // 7'li komboyu kullan.
            }
            else // Beceri seviyesi 0 ama bir þekilde aktif edilmiþse (hata önlemi)
            {
                currentCombo = normalCombo_5;
            }
        }
        else // Beceri KAPALI ise...
        {
            currentCombo = normalCombo_5; // Her zaman 5'li komboyu kullan.
        }

        ResetCombo();
    }
}