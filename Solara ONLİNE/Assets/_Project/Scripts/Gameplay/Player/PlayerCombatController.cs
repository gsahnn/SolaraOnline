using UnityEngine;
using System.Collections.Generic;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Verileri")]
    [Tooltip("Karakterin kullanabileceði tüm kombo setleri. Sýrasý önemlidir (0=Type1, 1=Type2...)")]
    [SerializeField] private List<ComboData> availableCombos;

    // Diðer script'ler
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    // Ýç sistem deðiþkenleri
    private int currentComboTypeIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        myStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        // Baþlangýçta ilk kombo türünü (Type 1) Animator'e bildir.
        animator.SetInteger("ComboType", currentComboTypeIndex);
    }

    // Bu fonksiyon, PlayerController'dan saldýrý komutu geldiðinde çaðrýlýr.
    public void SetAttackingState(bool isAttacking)
    {
        // Saldýrýya baþlarken hedefe dön.
        if (isAttacking && playerTargeting.currentTarget != null)
        {
            transform.LookAt(playerTargeting.currentTarget.position);
        }

        // Animator'e saldýrý durumunu bildir. Animator, bu bilgiye ve
        // mevcut 'ComboType' deðerine göre doðru kombo yolunu seçecektir.
        animator.SetBool("IsAttacking", isAttacking);
    }

    // Bu fonksiyon, UI'dan veya bir yetenekle çaðrýlabilir.
    public void CycleNextComboType()
    {
        // Bir sonraki kombo türüne geç.
        currentComboTypeIndex++;

        // Eðer listenin sonuna geldiysek, baþa dön.
        if (currentComboTypeIndex >= availableCombos.Count)
        {
            currentComboTypeIndex = 0;
        }

        // Yeni kombo türünü Animator'e bildir.
        animator.SetInteger("ComboType", currentComboTypeIndex);
        Debug.Log("Kombo Türü " + (currentComboTypeIndex) + " olarak ayarlandý."); // Index 0'dan baþladýðý için
    }

    // Bu, HER saldýrý animasyonunun VURUÞ ANINA event olarak eklenecek.
    public void AnimationEvent_DealDamage()
    {
        // Hasar verme mantýðý. Ýleride ComboData'dan hasar çarpaný alabilir.
        if (playerTargeting.currentTarget != null && playerTargeting.currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            targetStats.TakeDamage(damage, myStats);
        }
    }
}