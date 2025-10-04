using UnityEngine;

[CreateAssetMenu(fileName = "New AttackData", menuName = "Solara/Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Animasyon Bilgileri")]
    [Tooltip("Animator'de bu sald�r�y� tetikleyecek olan animasyon klipinin ad� veya state'in ad�.")]
    public string animationName;
    [Tooltip("Karakterin bu vuru�la ne kadar ileri at�laca��.")]
    public float forwardForce = 1f;
    [Tooltip("�leri at�lman�n ne kadar s�rece�i (saniye).")]
    public float forceDuration = 0.1f;

    [Header("Hasar Bilgileri")]
    [Tooltip("Bu sald�r�n�n temel hasar �arpan� (1.0 = %100 hasar).")]
    public float damageMultiplier = 1.0f;

    [Header("Efekt Bilgileri")]
    [Tooltip("Bu sald�r� hedefe isabet etti�inde onu yere d���r�r m�?")]
    public bool causesKnockdown = false;
}