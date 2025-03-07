using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Plants/PlantSO")]
public class PlantSO : ScriptableObject
{
     [System.Serializable]
    public class TilePreference
    {
        public TileSO preferredTile;
        public float weightBoost;
    }

    public TilePreference[] preferredTiles;
    public string plantName;
    public string plantFamily;
    public Sprite plantSprite;

    public int HP;
    public double hungerLevel;
    public double thirstLevel;

   
    

    public float weight;


    public float totalGrowthTicks;
    public float ticksNeededOne;
    public float ticksNeededTwo;
    public float ticksNeededThree;
    public int plantMaturity;
    public string plantMaturityName;
    public string nextPlantMaturityName;

    public int TicksNeeded()
    {
        if (totalGrowthTicks > ticksNeededOne)
        {

            return plantMaturity = 1;
        }
        else if (totalGrowthTicks > ticksNeededTwo)
        {
            return plantMaturity = 2;
        }
        else if (totalGrowthTicks > ticksNeededThree)
        {
            return plantMaturity = 3;
        }

        else return plantMaturity = 0;
    }

    public void SpreadSeeds(TileMono t)
    {
        if (plantMaturity == 2)
        {
            Debug.Log("working");
        }
        
    }
    
}
