using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Tile", menuName = "Grid/Tile")]
public class TileSO : ScriptableObject
{
    [Header("Tile Gen")]

    public string tileType;
    public Color tileColor;
    public float weight; // Base weight for this tile type

    [Header("Tile Base Info")]

    public int sunlightLevel; //1-10
    public int fertilityLevel;
    public int waterLevel;


    public List<PlantSlot> plantSlots = new List<PlantSlot>(5);
    public List<AnimalSlot> animalSlots = new List<AnimalSlot>(5);


    public class PlantSlot
    {
        public string plantID; // Plant ID (e.g., PLT00001)
        [Range(1, 3)] public int slotSize; // How many slots this plant takes (1-3)
    }

    [System.Serializable]
    public class AnimalSlot
    {
        public string animalID; // Animal ID (e.g., RAB00001)
        [Range(1, 3)] public int slotSize; // How many slots this animal takes (1-3)
    }


    // Map of preferred neighbor types and their weight adjustments
    [System.Serializable]
    public struct NeighborPreference
    {
        public TileSO neighborTile; // The preferred neighbor
        public float weightBoost;  // Weight adjustment for this neighbor
    }

    public NeighborPreference[] preferredNeighbors; // Array of preferences
}
