using UnityEngine;
using System.Collections.Generic;

// Bu bir ScriptableObject'tir. Proje dosyasý olarak yaþar.
[CreateAssetMenu(fileName = "New RankDatabase", menuName = "Solara/Database/Rank Database")]
public class RankDatabase : ScriptableObject
{
    public List<Rank> ranks;

    // Verilen puana karþýlýk gelen Rank verisini bulan fonksiyon.
    public Rank GetRankByPoints(int points)
    {
        foreach (Rank rank in ranks)
        {
            if (points >= rank.minPoints && points <= rank.maxPoints)
            {
                return rank;
            }
        }
        return null; // Uygun rütbe bulunamazsa boþ döndür.
    }
}