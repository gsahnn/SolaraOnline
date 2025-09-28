using UnityEngine;
using System.Collections.Generic; // List i�in gerekli

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Veri Dosyalar�")]
    [SerializeField] private ComboData normalCombo_5; // 5'li kombo
    [SerializeField] private ComboData specialCombo_6; // 6'l� kombo (Beceri Seviye 1)
    [SerializeField] private ComboData specialCombo_7; // 7'li kombo (Beceri Seviye 2)

    [Header("Beceri Durumu")]
    // Bu de�erler, karakterin verileriyle birlikte kaydedilip y�klenecek.
    [Tooltip("Kombo becerisinin mevcut seviyesi (0 = ��renilmemi�, 1 = seviye 1, 2 = seviye 2)")]
    public int comboSkillLevel = 0;

    [Tooltip("Oyuncu beceriyi a�t� m�?")]
    public bool isComboSkillActive = false;

    // Di�er script'ler
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    // �� sistem de�i�kenleri
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
        // Oyuna ba�larken, mevcut beceri durumuna g�re do�ru komboyu se�.
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

    // Bu fonksiyon PlayerController taraf�ndan �a�r�l�r.
    public void HandleAttackInput()
    {
        StartAttack();
    }

    private void StartAttack()
    {
        if (playerTargeting.currentTarget != null)
            transform.LookAt(playerTargeting.currentTarget.position);

        lastAttackTime = Time.time;

        // Mevcut kombo zincirinin bir sonraki ad�m�n� ate�le.
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

        // Hangi kombo setini kulland���m�z� bilmedi�imiz i�in en g�venli yol,
        // olas� t�m trigger'lar� s�f�rlamakt�r.
        for (int i = 1; i <= 7; i++)
        {
            animator.ResetTrigger("Attack" + i);
        }
        comboCounter = 0;
    }

    // Bu, kombo zinciri k�r�ld���nda Animator Event'i ile �a�r�l�r.
    public void AnimationEvent_ResetCombo()
    {
        ResetCombo();
    }

    // Bu, her sald�r� animasyonunun vuru� an�nda Animator Event'i ile �a�r�l�r.
    public void AnimationEvent_DealDamage()
    {
        // ... (Hasar verme mant��� ayn�, �arpan olmad��� i�in basit) ...
    }

    // --- YEN� VE �NEML� FONKS�YONLAR ---

    // Bu fonksiyon, oyuncu "Beceri Kitab�" okudu�unda �a�r�l�r.
    public void UpgradeComboSkill()
    {
        if (comboSkillLevel < 2) // Maksimum seviye 2
        {
            comboSkillLevel++;
            Debug.Log("Kombo Becerisi Seviye " + comboSkillLevel + " oldu!");
            // Becerinin durumu de�i�ti�i i�in, mevcut komboyu g�ncelle.
            UpdateCurrentCombo();
        }
    }

    // Bu fonksiyon, oyuncu beceriyi UI'dan a��p kapatt���nda �a�r�l�r.
    public void ToggleComboSkill()
    {
        // E�er beceri hi� ��renilmemi�se, a��p kapat�lamaz.
        if (comboSkillLevel == 0)
        {
            Debug.Log("Kombo Becerisi hen�z ��renilmemi�!");
            return;
        }

        isComboSkillActive = !isComboSkillActive;
        Debug.Log("Kombo Becerisi " + (isComboSkillActive ? "A�ILDI" : "KAPATILDI"));
        UpdateCurrentCombo();
    }

    // Bu fonksiyon, hangi kombo setinin kullan�laca��na karar veren ana mant�kt�r.
    private void UpdateCurrentCombo()
    {
        if (isComboSkillActive) // Beceri A�IK ise...
        {
            if (comboSkillLevel == 1)
            {
                currentCombo = specialCombo_6; // 6'l� komboyu kullan.
            }
            else if (comboSkillLevel == 2)
            {
                currentCombo = specialCombo_7; // 7'li komboyu kullan.
            }
            else // Beceri seviyesi 0 ama bir �ekilde aktif edilmi�se (hata �nlemi)
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