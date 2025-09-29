using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Verileri")]
    [Tooltip("Karakterin kullanabileceði tüm kombo setleri. Sýrasý önemlidir (0=Type1, 1=Type2...)")]
    [SerializeField] private List<ComboData> availableCombos;

    [Header("Savaþ Ayarlarý")]
    [Tooltip("Bir sonraki vuruþ için tanýnan maksimum süre (saniye).")]
    [SerializeField] private float comboResetTime = 1.5f;

    // Sistem Referanslarý
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;
    private Camera mainCamera;

    // Ýç Sistem Deðiþkenleri
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

        // Baþlangýçta ilk komboyu ata.
        if (availableCombos != null && availableCombos.Count > 0)
        {
            currentCombo = availableCombos[0];
        }
    }

    private void Start()
    {
        // Baþlangýçta kombo türünü Animator'e bildir.
        animator.SetInteger("ComboType", currentComboTypeIndex);
    }

    private void Update()
    {
        // Eðer son vuruþtan bu yana çok zaman geçtiyse, komboyu sýfýrla.
        if (comboCounter > 0 && Time.time - lastAttackTime > comboResetTime)
        {
            ResetCombo();
        }
    }

    // Bu fonksiyon PlayerController tarafýndan çaðrýlýr.
    public void HandleAttackInput()
    {
        // Kural: Karakter zaten bir "Action" (saldýrý/yetenek) animasyonunda ise, yeni saldýrý baþlatma.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Action")) return;

        comboCounter++;

        if (currentCombo == null || comboCounter > currentCombo.attacks.Count)
        {
            comboCounter = 1; // Zincir bittiyse veya tanýmsýzsa, baþa dön.
        }

        // Bir sonraki saldýrýnýn ID'sini ComboData'dan al.
        int actionIDToPlay = currentCombo.attacks[comboCounter - 1].actionID;

        SetAttackDirection();
        lastAttackTime = Time.time;

        // Animator'e "Þimdi bu ID'li eylemi yap" komutunu gönder.
        animator.SetInteger("ActionID", actionIDToPlay);

        // Komutun takýlý kalmamasý için bir sonraki frame'de sýfýrla.
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

    // UI'dan veya bir yetenekle çaðrýlabilir.
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
        Debug.Log("Kombo Türü Deðiþtirildi: Type " + currentComboTypeIndex);
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

    // Bu, HER saldýrý animasyonunun VURUÞ ANINA event olarak eklenecek.
    public void AnimationEvent_DealDamage()
    {
        if (comboCounter == 0) return;

        int currentAttackIndex = comboCounter - 1;
        // ... (Hasar verme kodu) ...
    }
}