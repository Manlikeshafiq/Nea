using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Tile", menuName = "Grid/Tile")]
public class TileSO : ScriptableObject
{
    
    [Header("Tile Gen")]
    public string tileBiome;
    public Color tileColor;
    public float weight;

    [Header("Tile Base Info")]
    public int sunlightLevel = 100; 
    public int fertilityLevel = 100;
    public int waterLeve = 100;


  


    [System.Serializable]
    public class NeighbourPreference
    {
        public TileSO neighbourTile;
        public float weightBoost;
    }

    public NeighbourPreference[] preferredneighbours;
}
