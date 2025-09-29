using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAttackingState(bool isAttacking)
    {
        animator.SetBool("IsAttacking", isAttacking);
    }

    public void AnimationEvent_DealDamage()
    {
        // ... (Hasar kodu ayný) ...
    }
}