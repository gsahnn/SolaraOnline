using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterStats), typeof(PlayerTargeting))]
public class PlayerCombatController : MonoBehaviour
{
    private Animator animator;
    private CharacterStats myStats;
    private PlayerTargeting playerTargeting;

    // Artık ComboData'ya veya herhangi bir kombo mantığına ihtiyacı yok.

    private void Awake()
    {
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStats>();
        playerTargeting = GetComponent<PlayerTargeting>();
    }

    // Bu fonksiyon, animasyonun hasar verme karesinde Event olarak çağrılır.
    // Hasar çarpanı gibi bilgiler, doğrudan event üzerinden bir AttackData asset'i ile alınabilir.
    public void AnimationEvent_DealDamage(AttackData attackData)
    {
        if (playerTargeting.currentTarget == null) return;

        CharacterStats targetStats = playerTargeting.currentTarget;

        // Hasar çarpanını artık event ile gelen AttackData'dan alıyoruz.
        float damageMultiplier = (attackData != null) ? attackData.damageMultiplier : 1.0f;
        bool causesKnockdown = (attackData != null) ? attackData.causesKnockdown : false;

        int damage = Mathf.RoundToInt(Random.Range(myStats.minDamage, myStats.maxDamage + 1) * damageMultiplier);
        targetStats.TakeDamage(damage, myStats, causesKnockdown);
    }
}
