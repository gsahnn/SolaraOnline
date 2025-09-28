using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ComboAttackData
{
    public string triggerName; // Bu adýmý tetikleyecek Animator Trigger'ý
    public float damageMultiplier = 1f;
}

[CreateAssetMenu(fileName = "New Combo Set", menuName = "Solara/Data/Combat/Combo Set")]
public class ComboData : ScriptableObject
{
    // Bu kombo setindeki saldýrýlarýn listesi
    public List<ComboAttackData> attacks;
}