using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Verileri")]
    [SerializeField] private ComboData normalCombo;
    [SerializeField] private ComboData specialCombo;
    [SerializeField] private float comboResetTime = 1.2f; // T�klamalar aras� maksimum bekleme s�resi

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
        // E�er son sald�r�dan bu yana �ok zaman ge�tiyse, komboyu s�f�rla.
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
        // E�er karakter zaten bir sald�r� animasyonu oynuyorsa, yeni sald�r� komutu alma.
        // Bu, kombonun ortas�nda spam'lemeyi engeller, zamanlama gerektirir.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            return;
        }

        StartAttack();
    }

    private void StartAttack()
    {
        // Bir �nceki sald�r�n�n zaman penceresi i�indeysek, komboya devam et.
        if (Time.time - lastAttackTime <= currentCombo.attacks[comboCounter].comboTimingWindow)
        {
            comboCounter++;
        }
        else // De�ilse, komboyu s�f�rdan ba�lat.
        {
            comboCounter = 1;
        }

        // Kombo zincirinin sonuna geldiysek, ba�a d�n.
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
        // Animator'deki trigger'lar� temizlemeye gerek yok, ��nk� art�k her sald�r�
        // state'inden Wait'e otomatik bir ��k�� var.
    }

    public void AnimationEvent_DealDamage()
    {
        // ... (Hasar verme kodu ayn�) ...
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
