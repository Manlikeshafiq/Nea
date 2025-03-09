using UnityEngine;


[CreateAssetMenu(fileName = "NewAnimal", menuName = "Animal/AnimalSO")]

public class AnimalSO : ScriptableObject
{
    public int animalID;
    public string animalType;

    public int HP;
    public double hungerLevel;
    public double thirstLevel;
    public float reproductionRate;
    public float energy;
    public int totalAnimalsOfType;
    public float weight;

    public Color colour;


    public bool dead = false;

    [System.Serializable]
    public class TilePreference
    {
        public TileSO spawnTiles;
        public int weightBoost;
    }

    public TilePreference[] spawnTiles;
}
