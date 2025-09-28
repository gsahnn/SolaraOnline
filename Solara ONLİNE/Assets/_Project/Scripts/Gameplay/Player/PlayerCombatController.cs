using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    // Bu script'in ihtiyaç duyduðu diðer bileþenlere referanslar.
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    private void Awake()
    {
        // Gerekli bileþenleri bu objenin üzerinden bul.
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        myStats = GetComponent<CharacterStats>();
    }

    // Bu fonksiyon PlayerController tarafýndan çaðrýlýr.
    public void SetAttackingState(bool isAttacking)
    {
        // Eðer saldýrý baþlýyorsa ve bir hedefimiz varsa, o hedefe doðru dön.
        if (isAttacking && playerTargeting.currentTarget != null)
        {
            transform.LookAt(playerTargeting.currentTarget.position);
        }

        // Animator'deki 'IsAttacking' parametresini güncelle.
        // Geri kalan her þeyi (hangi saldýrý animasyonunu oynatacaðýný) Animator kendisi halledecek.
        animator.SetBool("IsAttacking", isAttacking);
    }

    // Bu fonksiyon, HER saldýrý animasyonunun VURUÞ ANINA bir Animasyon Event'i olarak eklenecek.
    public void AnimationEvent_DealDamage()
    {
        // Eðer bir hedefimiz varsa ve bu hedefin caný varsa...
        if (playerTargeting.currentTarget != null && playerTargeting.currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            // Temel hasar hesaplamasý yap ve hedefe uygula.
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            targetStats.TakeDamage(damage, myStats);
        }
    }

    // Not: Bu script artýk kombo sayacý veya zamanlayýcý tutmaz.
    // Tüm kombo mantýðý Animator Controller'a taþýnmýþtýr. Bu, kodu çok daha basit hale getirir.
}