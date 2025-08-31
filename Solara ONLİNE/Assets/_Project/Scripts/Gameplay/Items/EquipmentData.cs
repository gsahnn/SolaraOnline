// EquipmentData.cs
using UnityEngine;

// Þimdilik bu script'ten doðrudan yeni asset oluþturmayacaðýz, o yüzden menu satýrý yok.
public class EquipmentData : ItemData
{
    [Header("Ekipman Statlarý")]
    public int bonusHealth;
    public int bonusMana;
    public int bonusStrength;
    public int bonusDefense;
    // Ýleride daha fazla stat eklenebilir...
}