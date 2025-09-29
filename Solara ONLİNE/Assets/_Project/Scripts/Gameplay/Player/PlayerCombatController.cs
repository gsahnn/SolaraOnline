using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Verileri")]
    [Tooltip("Karakterin kullanabilece�i t�m kombo setleri. S�ras� �nemlidir (0=Type1, 1=Type2...)")]
    [SerializeField] private List<ComboData> availableCombos;

    [Header("Sava� Ayarlar�")]
    [Tooltip("Bir sonraki vuru� i�in tan�nan maksimum s�re (saniye).")]
    [SerializeField] private float comboResetTime = 1.5f;

    // Sistem Referanslar�
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;
    private Camera mainCamera;

    // �� Sistem De�i�kenleri
    private ComboData currentCombo;
    private int currentComboTypeIndex = 0;
    private int comboCounter = 0;
    private float lastAttackTime = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        myStats = GetComponent<CharacterStats>();
        mainCamera = Camera.main;

        // Ba�lang��ta ilk komboyu ata.
        if (availableCombos != null && availableCombos.Count > 0)
        {
            currentCombo = availableCombos[0];
        }
    }

    private void Start()
    {
        // Ba�lang��ta kombo t�r�n� Animator'e bildir.
        animator.SetInteger("ComboType", currentComboTypeIndex);
    }

    private void Update()
    {
        // E�er son vuru�tan bu yana �ok zaman ge�tiyse, komboyu s�f�rla.
        if (comboCounter > 0 && Time.time - lastAttackTime > comboResetTime)
        {
            ResetCombo();
        }
    }

    // Bu fonksiyon PlayerController taraf�ndan �a�r�l�r.
    public void HandleAttackInput()
    {
        // Kural: Karakter zaten bir "Action" (sald�r�/yetenek) animasyonunda ise, yeni sald�r� ba�latma.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Action")) return;

        comboCounter++;

        if (currentCombo == null || comboCounter > currentCombo.attacks.Count)
        {
            comboCounter = 1; // Zincir bittiyse veya tan�ms�zsa, ba�a d�n.
        }

        // Bir sonraki sald�r�n�n ID'sini ComboData'dan al.
        int actionIDToPlay = currentCombo.attacks[comboCounter - 1].actionID;

        SetAttackDirection();
        lastAttackTime = Time.time;

        // Animator'e "�imdi bu ID'li eylemi yap" komutunu g�nder.
        animator.SetInteger("ActionID", actionIDToPlay);

        // Komutun tak�l� kalmamas� i�in bir sonraki frame'de s�f�rla.
        StartCoroutine(ResetActionID());
    }

    private IEnumerator ResetActionID()
    {
        yield return null; // Sadece 1 frame bekle.
        animator.SetInteger("ActionID", 0);
    }

    private void ResetCombo()
    {
        comboCounter = 0;
    }

    // UI'dan veya bir yetenekle �a�r�labilir.
    public void CycleNextComboType()
    {
        currentComboTypeIndex++;
        if (currentComboTypeIndex >= availableCombos.Count)
        {
            currentComboTypeIndex = 0;
        }
        currentCombo = availableCombos[currentComboTypeIndex];
        animator.SetInteger("ComboType", currentComboTypeIndex);
        ResetCombo();
        Debug.Log("Kombo T�r� De�i�tirildi: Type " + currentComboTypeIndex);
    }

    private void SetAttackDirection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Default")))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else if (playerTargeting.currentTarget != null)
        {
            transform.LookAt(playerTargeting.currentTarget.position);
        }
    }

    // Bu, HER sald�r� animasyonunun VURU� ANINA event olarak eklenecek.
    public void AnimationEvent_DealDamage()
    {
        if (comboCounter == 0) return;

        int currentAttackIndex = comboCounter - 1;
        // ... (Hasar verme kodu) ...
    }
}