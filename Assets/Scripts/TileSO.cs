using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Tile", menuName = "Grid/Tile")]
public class TileSO : ScriptableObject
{
    
    public string tileBiome;
    public Color tileColor;
    public float weight;

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
