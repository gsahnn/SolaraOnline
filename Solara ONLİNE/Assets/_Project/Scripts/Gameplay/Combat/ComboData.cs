using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ComboAttackData
{
    public string triggerName; // Bu art�k kullan�lm�yor, ama referans i�in kalabilir.
    public int actionID; // BU �OK �NEML�! (Attack1 i�in 1, Attack2 i�in 2 vb.)
    public float damageMultiplier = 1.0f;
}

[CreateAssetMenu(fileName = "New Combo Set", menuName = "Solara/Data/Combat/Combo Set")]
public class ComboData : ScriptableObject
{
    // Bu kombo setindeki sald�r�lar�n listesi
    public List<ComboAttackData> attacks;
}