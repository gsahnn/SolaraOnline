using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Verileri")]
    [SerializeField] private ComboData normalCombo;
    [SerializeField] private ComboData specialCombo;
    [SerializeField] private float comboResetTime = 1.2f; // Týklamalar arasý maksimum bekleme süresi

    public bool isSpecialComboActive = false;

    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    private ComboData currentCombo;
    private int comboCounter = 0;
    private float lastAttackTime = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        myStats = GetComponent<CharacterStats>();
        UpdateCurrentCombo();
    }

    private void Update()
    {
        // Eðer son saldýrýdan bu yana çok zaman geçtiyse, komboyu sýfýrla.
        if (Time.time - lastAttackTime > comboResetTime)
        {
            if (comboCounter > 0)
            {
                ResetCombo();
            }
        }
    }

    public void HandleAttackInput()
    {
        // Eðer karakter zaten bir saldýrý animasyonu oynuyorsa, yeni saldýrý komutu alma.
        // Bu, kombonun ortasýnda spam'lemeyi engeller, zamanlama gerektirir.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            return;
        }

        StartAttack();
    }

    private void StartAttack()
    {
        // Bir önceki saldýrýnýn zaman penceresi içindeysek, komboya devam et.
        if (Time.time - lastAttackTime <= currentCombo.attacks[comboCounter].comboTimingWindow)
        {
            comboCounter++;
        }
        else // Deðilse, komboyu sýfýrdan baþlat.
        {
            comboCounter = 1;
        }

        // Kombo zincirinin sonuna geldiysek, baþa dön.
        if (comboCounter > currentCombo.attacks.Count)
        {
            comboCounter = 1;
        }

        if (playerTargeting.currentTarget != null)
            transform.LookAt(playerTargeting.currentTarget.position);

        lastAttackTime = Time.time;

        string triggerToFire = currentCombo.attacks[comboCounter - 1].triggerName;
        animator.SetTrigger(triggerToFire);
    }

    private void ResetCombo()
    {
        comboCounter = 0;
        // Animator'deki trigger'larý temizlemeye gerek yok, çünkü artýk her saldýrý
        // state'inden Wait'e otomatik bir çýkýþ var.
    }

    public void AnimationEvent_DealDamage()
    {
        // ... (Hasar verme kodu ayný) ...
    }

    public void SetSpecialComboActive(bool isActive)
    {
        isSpecialComboActive = isActive;
        UpdateCurrentCombo();
        ResetCombo();
    }

    private void UpdateCurrentCombo()
    {
        currentCombo = isSpecialComboActive ? specialCombo : normalCombo;
    }
}
