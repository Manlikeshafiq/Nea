using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Tile", menuName = "Grid/Tile")]
public class TileSO : ScriptableObject
{
    TileClones tileMono = new TileClones();

    [Header("Tile Gen")]
    public string tileBiome;
    public Color tileColor;
    public float weight;

    [Header("Tile Base Info")]
    public int sunlightLevel = 100; 
    public int fertilityLevel = 100;
    public int waterLevel = 100;


  


    [System.Serializable]
    public class NeighborPreference
    {
        public TileSO neighborTile;
        public float weightBoost;
    }

    public NeighborPreference[] preferredNeighbors;
}
