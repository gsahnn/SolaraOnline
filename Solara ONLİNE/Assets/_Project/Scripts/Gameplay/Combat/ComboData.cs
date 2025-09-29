using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ComboAttackData
{
    public string triggerName; // Bu artýk kullanýlmýyor, ama referans için kalabilir.
    public int actionID; // BU ÇOK ÖNEMLÝ! (Attack1 için 1, Attack2 için 2 vb.)
    public float damageMultiplier = 1.0f;
}

[CreateAssetMenu(fileName = "New Combo Set", menuName = "Solara/Data/Combat/Combo Set")]
public class ComboData : ScriptableObject
{
    // Bu kombo setindeki saldýrýlarýn listesi
    public List<ComboAttackData> attacks;
}