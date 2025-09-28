using UnityEngine;
using System.Collections.Generic;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Veri Dosyalar�")]
    [Tooltip("Karakterin kullanabilece�i t�m kombo setleri.")]
    [SerializeField] private List<ComboData> availableCombos;

    // Di�er script'ler
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    // �� sistem de�i�kenleri
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
        // Ba�lang��ta ilk komboyu ata
        if (availableCombos != null && availableCombos.Count > 0)
        {
            currentCombo = availableCombos[0];
        }
    }

    private void Update()
    {
        // E�er son vuru�tan bu yana �ok zaman ge�tiyse, komboyu s�f�rla.
        // Bu, Attack->Movement ge�i�indeki Exit Time'a ek bir g�venlik katman�d�r.
        if (comboCounter > 0 && Time.time - lastAttackTime > 1.5f) // 1.5 saniye bekleme s�resi
        {
            ResetCombo();
        }
    }

    // Bu fonksiyon PlayerController taraf�ndan �a�r�l�r.
    public void HandleAttackInput()
    {
        // E�er karakter zaten bir sald�r� animasyonu oynuyorsa, yeni bir sald�r� komutu alma.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            return;
        }

        // Bir �nceki sald�r�n�n bitimine yak�n m� t�kland�?
        // Bu zamanlama, kombonun ak�c� hissettirmesi i�in ayarlanabilir.
        if (comboCounter > 0 && Time.time - lastAttackTime > currentCombo.attacks[comboCounter - 1].comboTimingWindow)
        {
            // �ok ge� t�kland�, komboyu s�f�rdan ba�lat.
            ResetCombo();
        }

        StartOrContinueCombo();
    }

    private void StartOrContinueCombo()
    {
        if (playerTargeting.currentTarget != null)
            transform.LookAt(playerTargeting.currentTarget.position);

        lastAttackTime = Time.time;

        // Bir sonraki kombo ad�m�na ge�
        comboCounter++;

        // E�er mevcut kombonun sonuna geldiysek, s�f�rla ve ilk ad�ma d�n.
        if (comboCounter > currentCombo.attacks.Count)
        {
            comboCounter = 1;
        }

        // ComboData'dan do�ru trigger'� al ve ate�le
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
        // ... (Hasar verme kodu ayn�, �arpan olmad��� i�in basit) ...
    }

    // Bu fonksiyon, UI'dan veya bir yetenekle �a�r�labilir.
    public void ChangeComboType()
    {
        currentComboTypeIndex++;
        if (currentComboTypeIndex >= availableCombos.Count)
        {
            currentComboTypeIndex = 0;
        }
        currentCombo = availableCombos[currentComboTypeIndex];
        ResetCombo();
        Debug.Log("Kombo T�r� De�i�tirildi: " + currentCombo.name);
    }
}