using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    // Bu script'in ihtiya� duydu�u di�er bile�enlere referanslar.
    private Animator animator;
    private PlayerTargeting playerTargeting;
    private CharacterStats myStats;

    private void Awake()
    {
        // Gerekli bile�enleri bu objenin �zerinden bul.
        animator = GetComponent<Animator>();
        playerTargeting = GetComponent<PlayerTargeting>();
        myStats = GetComponent<CharacterStats>();
    }

    // Bu fonksiyon PlayerController taraf�ndan �a�r�l�r.
    public void SetAttackingState(bool isAttacking)
    {
        // E�er sald�r� ba�l�yorsa ve bir hedefimiz varsa, o hedefe do�ru d�n.
        if (isAttacking && playerTargeting.currentTarget != null)
        {
            transform.LookAt(playerTargeting.currentTarget.position);
        }

        // Animator'deki 'IsAttacking' parametresini g�ncelle.
        // Geri kalan her �eyi (hangi sald�r� animasyonunu oynataca��n�) Animator kendisi halledecek.
        animator.SetBool("IsAttacking", isAttacking);
    }

    // Bu fonksiyon, HER sald�r� animasyonunun VURU� ANINA bir Animasyon Event'i olarak eklenecek.
    public void AnimationEvent_DealDamage()
    {
        // E�er bir hedefimiz varsa ve bu hedefin can� varsa...
        if (playerTargeting.currentTarget != null && playerTargeting.currentTarget.TryGetComponent(out CharacterStats targetStats))
        {
            // Temel hasar hesaplamas� yap ve hedefe uygula.
            int damage = Random.Range(myStats.minDamage, myStats.maxDamage + 1);
            targetStats.TakeDamage(damage, myStats);
        }
    }

    // Not: Bu script art�k kombo sayac� veya zamanlay�c� tutmaz.
    // T�m kombo mant��� Animator Controller'a ta��nm��t�r. Bu, kodu �ok daha basit hale getirir.
}