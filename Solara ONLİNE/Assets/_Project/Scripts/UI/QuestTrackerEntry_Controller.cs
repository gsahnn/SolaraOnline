// QuestTrackerEntry_Controller.cs
using UnityEngine;
using TMPro;

public class QuestTrackerEntry_Controller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questProgressText;

    public void SetQuest(QuestStatus questStatus)
    {
        questNameText.text = questStatus.questData.questName;

        string progress = $"{questStatus.currentAmount} / {questStatus.questData.requiredAmount}";
        questProgressText.text = progress;

        // G�rev tamamland�ysa, metnin rengini ye�il yapal�m.
        if (questStatus.isCompleted)
        {
            questNameText.color = Color.green;
            questProgressText.color = Color.green;
        }
        else
        {
            questNameText.color = Color.white;
            questProgressText.color = Color.white;
        }
    }
}