// QuestData.cs
using UnityEngine;

// G�revin amac�n�n ne oldu�unu belirleyen enum.
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
    public string targetName; // �rn: "Kurt" veya "Kurt Postu"

    [Header("�d�ller")]
    public int experienceReward;
    public int goldReward;
    // public ItemData itemReward; // �leride e�ya �d�l� de ekleyebiliriz.
}