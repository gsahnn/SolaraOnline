using UnityEngine;
using System.Collections.Generic;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Kombo Verileri")]
    [Tooltip("Karakterin kullanabilece�i t�m kombo setleri. S�ras� �nemlidir (0=Type1, 1=Type2...)")]
    [SerializeField] private List<ComboData> availableCombos;

    // Di�er script'ler
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    // �� sistem de�i�kenleri
    private int currentComboTypeIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        myStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        // Ba�lang��ta ilk kombo t�r�n� (Type 1) Animator'e bildir.
        animator.SetInteger("ComboType", currentComboTypeIndex);
    }

    // Bu fonksiyon, PlayerController'dan sald�r� komutu geldi�inde �a�r�l�r.
    public void SetAttackingState(bool isAttacking)
    {
        // Sald�r�ya ba�larken hedefe d�n.
        if (isAttacking && playerTargeting.currentTarget != null)
        {
            transform.LookAt(playerTargeting.currentTarget.position);
        }

        // Animator'e sald�r� durumunu bildir. Animator, bu bilgiye ve
        // mevcut 'ComboType' de�erine g�re do�ru kombo yolunu se�ecektir.
        animator.SetBool("IsAttacking", isAttacking);
    }

    // Bu fonksiyon, UI'dan veya bir yetenekle �a�r�labilir.
    public void CycleNextComboType()
    {
        // Bir sonraki kombo t�r�ne ge�.
        currentComboTypeIndex++;

        // E�er listenin sonuna geldiysek, ba�a d�n.
        if (currentComboTypeIndex >= availableCombos.Count)
        {
            currentComboTypeIndex = 0;
        }

        // Yeni kombo t�r�n� Animator'e bildir.
        animator.SetInteger("ComboType", currentComboTypeIndex);
        Debug.Log("Kombo T�r� " + (currentComboTypeIndex) + " olarak ayarland�."); // Index 0'dan ba�lad��� i�in
    }

    // Bu, HER sald�r� animasyonunun VURU� ANINA event olarak eklenecek.
    public void AnimationEvent_DealDamage()
    {
        // Hasar verme mant���. �leride ComboData'dan hasar �arpan� alabilir.
        if (playerTargeting.currentTarget != null && playerTargeting.currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            targetStats.TakeDamage(damage, myStats);
        }
    }
}