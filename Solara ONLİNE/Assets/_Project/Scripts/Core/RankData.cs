using UnityEngine;

// Bu bir [System.Serializable]'dýr, MonoBehaviour DEÐÝL.
// Bu sayede RankDatabase'in Inspector'unda bir liste olarak görünebilir.
[System.Serializable]
public class Rank
{
    public string rankName;
    public Color rankColor = Color.white;
    public int minPoints;
    public int maxPoints;
}