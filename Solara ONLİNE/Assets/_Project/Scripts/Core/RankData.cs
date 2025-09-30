using UnityEngine;

// Bu bir [System.Serializable]'d�r, MonoBehaviour DE��L.
// Bu sayede RankDatabase'in Inspector'unda bir liste olarak g�r�nebilir.
[System.Serializable]
public class Rank
{
    public string rankName;
    public Color rankColor = Color.white;
    public int minPoints;
    public int maxPoints;
}