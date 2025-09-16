// QuestData.cs
using UnityEngine;

public enum QuestGoalType { Kill, Gather }

[CreateAssetMenu(fileName = "New Quest", menuName = "Solara/Data/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Genel Bilgiler")]
    public string questName;
    [TextArea(3, 5)] public string description;

    [Header("Diyaloglar")] // <-- YENÝ BÖLÜM
    [TextArea(3, 5)] public string startDialogue; // Görevi almadan önce NPC'nin söyleyeceði
    [TextArea(3, 5)] public string inProgressDialogue; // Görev aktifken NPC'nin söyleyeceði
    [TextArea(3, 5)] public string completedDialogue; // Görev tamamlandýðýnda NPC'nin söyleyeceði

    [Header("Hedef")]
    public QuestGoalType goalType;
    public int requiredAmount;
    public string targetName;

    [Header("Ödüller")]
    public int experienceReward;
    public int goldReward;
}