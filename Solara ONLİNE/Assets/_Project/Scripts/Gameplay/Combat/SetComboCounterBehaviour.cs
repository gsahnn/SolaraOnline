using UnityEngine;

// Bu script, bir animasyon durumuna eklendi�inde �al���r.
public class SetComboCounterBehaviour : StateMachineBehaviour
{
    [Tooltip("Bu animasyon durumu aktif oldu�unda ComboCounter parametresi bu de�ere ayarlanacak.")]
    public int counterValue;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // Bu fonksiyon, bu animasyon durumu ba�lar ba�lamaz B�R KERE �al���r.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Animator'deki "ComboCounter" adl� integer parametresini,
        // Inspector'da belirledi�imiz "counterValue" de�erine ayarla.
        animator.SetInteger("ComboCounter", counterValue);
    }
}