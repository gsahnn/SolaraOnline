using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Rank Database", menuName = "Solara/Data/Characters/Rank Database")]
public class RankDatabase : ScriptableObject
{
    public List<RankData> allRanks;

    // Verilen puana uygun olan r�tbeyi bulup d�nd�r�r.
    public RankData GetRankForAlignment(int alignment)
    {
        return allRanks.FirstOrDefault(rank => alignment >= rank.minAlignment && alignment <= rank.maxAlignment);
    }
}