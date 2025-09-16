// QuestData.cs
using UnityEngine;

public enum QuestGoalType { Kill, Gather }

[CreateAssetMenu(fileName = "New Quest", menuName = "Solara/Data/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Genel Bilgiler")]
    public string questName;
    [TextArea(3, 5)] public string description;

    [Header("Diyaloglar")] // <-- YEN� B�L�M
    [TextArea(3, 5)] public string startDialogue; // G�revi almadan �nce NPC'nin s�yleyece�i
    [TextArea(3, 5)] public string inProgressDialogue; // G�rev aktifken NPC'nin s�yleyece�i
    [TextArea(3, 5)] public string completedDialogue; // G�rev tamamland���nda NPC'nin s�yleyece�i

    [Header("Hedef")]
    public QuestGoalType goalType;
    public int requiredAmount;
    public string targetName;

    [Header("�d�ller")]
    public int experienceReward;
    public int goldReward;
}