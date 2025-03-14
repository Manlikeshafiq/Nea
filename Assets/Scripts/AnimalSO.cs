using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalSO", menuName = "Scriptable Objects/AnimalSO")]
public class AnimalSO : ScriptableObject
{
    [System.Serializable]
    public class TilePreference
    {
        public TileSO preferredTile;
        public float weightBoost;
    }


    public List<PlantSO> plantSOList;

    public TilePreference[] preferredTiles;


    [System.Serializable]
    public class EdiblePlants
    {
        public PlantSO ediblePlant;
        public float weightBoost;
    }

    public EdiblePlants[] ediblePlants;

    public TileMono spawnTile;
    public string animalName;
    public int animalID;

    public Sprite animalSprite;

    public int HP = 100;
    public double hungerLevel = 100;
    public double thirstLevel = 100;
    public bool dead = false;
    public float attackproficiency = 9;



    public float weight;


    public float totalGrowthTicks;
    public float ticksNeededOne;
    public float ticksNeededTwo;
    public float ticksNeededThree;
    public float ticksNeededFour;

    public int animalMaturity = 0;
    public string animalMaturityName;
    public string nextAnimalMaturityName;
    public int seedsPerYear = 2000;

    public int TicksNeeded()
    {
        if (totalGrowthTicks >= ticksNeededOne && totalGrowthTicks < ticksNeededTwo)
        {

            return animalMaturity = 1;
        }
        if (totalGrowthTicks >= ticksNeededTwo && totalGrowthTicks < ticksNeededThree)
        {

            return animalMaturity = 2;
        }
        if (totalGrowthTicks >= ticksNeededThree && totalGrowthTicks < ticksNeededFour)
        {
            return animalMaturity = 3;
        }

        if (totalGrowthTicks >= ticksNeededFour)
        {
            return animalMaturity = 4;
        }
        else return animalMaturity = 0;


    }

    public AnimalSO CloneAnimalSO(AnimalSO data)
    {
        AnimalSO clonedAnimal = ScriptableObject.CreateInstance<AnimalSO>();


        //COPY ANY NEW DATA
        clonedAnimal.animalName = data.animalName;
        clonedAnimal.animalID = data.animalID;
        clonedAnimal.animalSprite = data.animalSprite;
        clonedAnimal.HP = data.HP;
        clonedAnimal.hungerLevel = data.hungerLevel;
        clonedAnimal.thirstLevel = data.thirstLevel;
        clonedAnimal.dead = data.dead;
        clonedAnimal.weight = data.weight;
        clonedAnimal.totalGrowthTicks = data.totalGrowthTicks;
        clonedAnimal.ticksNeededOne = data.ticksNeededOne;
        clonedAnimal.ticksNeededTwo = data.ticksNeededTwo;
        clonedAnimal.ticksNeededThree = data.ticksNeededThree;
        clonedAnimal.ticksNeededFour = data.ticksNeededFour;
        clonedAnimal.animalMaturity = data.animalMaturity;
        clonedAnimal.animalMaturityName = data.animalMaturityName;
        clonedAnimal.nextAnimalMaturityName = data.nextAnimalMaturityName;
        clonedAnimal.seedsPerYear = data.seedsPerYear;
        clonedAnimal.preferredTiles = data.preferredTiles;
        clonedAnimal.spawnTile = data.spawnTile;
        clonedAnimal.ediblePlants = data.ediblePlants;
        clonedAnimal.attackproficiency = data.attackproficiency;


        return clonedAnimal;
    }
}