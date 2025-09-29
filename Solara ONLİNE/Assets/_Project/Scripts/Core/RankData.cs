using UnityEngine;

[CreateAssetMenu(fileName = "New Rank", menuName = "Solara/Data/Characters/Rank")]
public class RankData : ScriptableObject
{
    public string rankName;
    public Color rankColor = Color.white;
    public int minAlignment; // Bu r�tbe i�in gereken minimum puan
    public int maxAlignment; // Bu r�tbe i�in gereken maksimum puan
}