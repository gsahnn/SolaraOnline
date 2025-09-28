using UnityEngine;
using System.Collections.Generic;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Veri Dosyalarý")]
    [Tooltip("Karakterin kullanabileceði tüm kombo setleri.")]
    [SerializeField] private List<ComboData> availableCombos;

    // Diðer script'ler
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    // Ýç sistem deðiþkenleri
    private ComboData currentCombo;
    private int currentComboTypeIndex = 0;
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
        // Baþlangýçta ilk komboyu ata
        if (availableCombos != null && availableCombos.Count > 0)
        {
            currentCombo = availableCombos[0];
        }
    }

    private void Update()
    {
        // Eðer son vuruþtan bu yana çok zaman geçtiyse, komboyu sýfýrla.
        // Bu, Attack->Movement geçiþindeki Exit Time'a ek bir güvenlik katmanýdýr.
        if (comboCounter > 0 && Time.time - lastAttackTime > 1.5f) // 1.5 saniye bekleme süresi
        {
            ResetCombo();
        }
    }

    // Bu fonksiyon PlayerController tarafýndan çaðrýlýr.
    public void HandleAttackInput()
    {
        // Eðer karakter zaten bir saldýrý animasyonu oynuyorsa, yeni bir saldýrý komutu alma.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            return;
        }

        // Bir önceki saldýrýnýn bitimine yakýn mý týklandý?
        // Bu zamanlama, kombonun akýcý hissettirmesi için ayarlanabilir.
        if (comboCounter > 0 && Time.time - lastAttackTime > currentCombo.attacks[comboCounter - 1].comboTimingWindow)
        {
            // Çok geç týklandý, komboyu sýfýrdan baþlat.
            ResetCombo();
        }

        StartOrContinueCombo();
    }

    private void StartOrContinueCombo()
    {
        if (playerTargeting.currentTarget != null)
            transform.LookAt(playerTargeting.currentTarget.position);

        lastAttackTime = Time.time;

        // Bir sonraki kombo adýmýna geç
        comboCounter++;

        // Eðer mevcut kombonun sonuna geldiysek, sýfýrla ve ilk adýma dön.
        if (comboCounter > currentCombo.attacks.Count)
        {
            comboCounter = 1;
        }

        // ComboData'dan doðru trigger'ý al ve ateþle
        string triggerToFire = currentCombo.attacks[comboCounter - 1].triggerName;
        animator.SetTrigger(triggerToFire);
    }

    private void ResetCombo()
    {
        if (comboCounter == 0) return;
        comboCounter = 0;
    }

    public void AnimationEvent_DealDamage()
    {
        // ... (Hasar verme kodu ayný, çarpan olmadýðý için basit) ...
    }

    // Bu fonksiyon, UI'dan veya bir yetenekle çaðrýlabilir.
    public void ChangeComboType()
    {
        currentComboTypeIndex++;
        if (currentComboTypeIndex >= availableCombos.Count)
        {
            currentComboTypeIndex = 0;
        }
        currentCombo = availableCombos[currentComboTypeIndex];
        ResetCombo();
        Debug.Log("Kombo Türü Deðiþtirildi: " + currentCombo.name);
    }
}