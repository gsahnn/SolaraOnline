using UnityEngine;

[CreateAssetMenu(fileName = "New AttackData", menuName = "Solara/Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Animasyon Bilgileri")]
    [Tooltip("Animator'de bu saldýrýyý tetikleyecek olan animasyon klipinin adý veya state'in adý.")]
    public string animationName;
    [Tooltip("Karakterin bu vuruþla ne kadar ileri atýlacaðý.")]
    public float forwardForce = 1f;
    [Tooltip("Ýleri atýlmanýn ne kadar süreceði (saniye).")]
    public float forceDuration = 0.1f;

    [Header("Hasar Bilgileri")]
    [Tooltip("Bu saldýrýnýn temel hasar çarpaný (1.0 = %100 hasar).")]
    public float damageMultiplier = 1.0f;

    [Header("Efekt Bilgileri")]
    [Tooltip("Bu saldýrý hedefe isabet ettiðinde onu yere düþürür mü?")]
    public bool causesKnockdown = false;
}