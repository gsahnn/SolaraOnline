// EquipmentData.cs
using UnityEngine;

// �imdilik bu script'ten do�rudan yeni asset olu�turmayaca��z, o y�zden menu sat�r� yok.
public class EquipmentData : ItemData
{
    [Header("Ekipman Statlar�")]
    public int bonusHealth;
    public int bonusMana;
    public int bonusStrength;
    public int bonusDefense;
    // �leride daha fazla stat eklenebilir...
}