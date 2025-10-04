using UnityEngine;

// Bu script, bir animasyon durumuna eklendiðinde çalýþýr.
public class SetComboCounterBehaviour : StateMachineBehaviour
{
    [Tooltip("Bu animasyon durumu aktif olduðunda ComboCounter parametresi bu deðere ayarlanacak.")]
    public int counterValue;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // Bu fonksiyon, bu animasyon durumu baþlar baþlamaz BÝR KERE çalýþýr.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Animator'deki "ComboCounter" adlý integer parametresini,
        // Inspector'da belirlediðimiz "counterValue" deðerine ayarla.
        animator.SetInteger("ComboCounter", counterValue);
    }
}