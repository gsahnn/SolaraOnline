using UnityEngine;

[CreateAssetMenu(fileName = "New Rank", menuName = "Solara/Data/Characters/Rank")]
public class RankData : ScriptableObject
{
    public string rankName;
    public Color rankColor = Color.white;
    public int minAlignment; // Bu rütbe için gereken minimum puan
    public int maxAlignment; // Bu rütbe için gereken maksimum puan
}