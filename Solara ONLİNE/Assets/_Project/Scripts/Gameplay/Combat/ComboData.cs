using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

// Bu script, projenizde AttackData.cs'in de bulunmasýný gerektirir.
[CreateAssetMenu(fileName = "New ComboData", menuName = "Solara/Combat/Combo Data")]
public class ComboData : ScriptableObject
{
    [Tooltip("Bu kombo zincirini oluþturan saldýrýlarýn sýrasý.")]
    public List<AttackData> comboSequence;
}