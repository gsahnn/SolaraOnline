using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ComboAttackData
{
    public string triggerName;
    [Tooltip("Bir sonraki komboya geçmek için tanýnan zaman penceresi (saniye)")]
    


[CreateAssetMenu(fileName = "New Combo Set", menuName = "Solara/Data/Combat/Combo Set")]
public class ComboData : ScriptableObject
{
    // Bu kombo setindeki saldýrýlarýn listesi (5'li veya 7'li)
    public List<ComboAttackData> attacks;
    }
}