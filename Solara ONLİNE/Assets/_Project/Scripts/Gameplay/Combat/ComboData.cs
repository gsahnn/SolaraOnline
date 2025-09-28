using UnityEngine;
using System.Collections.Generic;

// Tek bir saldýrý adýmýnýn özelliklerini tutar.
[System.Serializable]
public class ComboAttackData
{
    // Animator'de bu saldýrýyý hangi trigger'ýn tetikleyeceðini belirtir.
    public string triggerName;
    // Bir sonraki komboya geçmek için oyuncuya tanýnan zaman penceresi (saniye).
    public float comboTimingWindow = 0.7f;
    // Bu vuruþun temel hasar çarpaný.
    public float damageMultiplier = 1.0f;
}

[CreateAssetMenu(fileName = "New Combo Set", menuName = "Solara/Data/Combat/Combo Set")]
public class ComboData : ScriptableObject
{
    public List<ComboAttackData> attacks;
}