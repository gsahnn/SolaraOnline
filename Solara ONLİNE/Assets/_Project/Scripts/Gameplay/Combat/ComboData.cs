using UnityEngine;
using System.Collections.Generic;

// Tek bir sald�r� ad�m�n�n �zelliklerini tutar.
[System.Serializable]
public class ComboAttackData
{
    // Animator'de bu sald�r�y� hangi trigger'�n tetikleyece�ini belirtir.
    public string triggerName;
    // Bir sonraki komboya ge�mek i�in oyuncuya tan�nan zaman penceresi (saniye).
    public float comboTimingWindow = 0.7f;
    // Bu vuru�un temel hasar �arpan�.
    public float damageMultiplier = 1.0f;
}

[CreateAssetMenu(fileName = "New Combo Set", menuName = "Solara/Data/Combat/Combo Set")]
public class ComboData : ScriptableObject
{
    public List<ComboAttackData> attacks;
}