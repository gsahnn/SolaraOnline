// QuestData.cs
using UnityEngine;

// Görevin amacýnýn ne olduðunu belirleyen enum.
public enum QuestGoalType
{
    Kill,
    Gather
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Solara/Data/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Genel Bilgiler")]
    public string questName;
    [TextArea(3, 5)] public string description;

    [Header("Hedef")]
    public QuestGoalType goalType;
    public int requiredAmount;
    public string targetName; // Örn: "Kurt" veya "Kurt Postu"

    [Header("Ödüller")]
    public int experienceReward;
    public int goldReward;
    // public ItemData itemReward; // Ýleride eþya ödülü de ekleyebiliriz.
}