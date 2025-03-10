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
    public int plantID;

    public string plantFamily;
    public Sprite plantSprite;

    public double HP;
    public double hungerLevel;
    public double thirstLevel;
    public bool dead = false;

   
    

    public float weight;


    public float totalGrowthTicks;
    public float ticksNeededOne;
    public float ticksNeededTwo;
    public float ticksNeededThree;
    public float ticksNeededFour;

    public int plantMaturity = 0;
    public string plantMaturityName;
    public string nextPlantMaturityName;
    public int seedsPerYear =2000;

    public int TicksNeeded()
    {
        if (totalGrowthTicks >= ticksNeededOne && totalGrowthTicks < ticksNeededTwo)
        {

            return plantMaturity = 1;
        }
        if (totalGrowthTicks >= ticksNeededTwo && totalGrowthTicks < ticksNeededThree)
        {

            return plantMaturity = 2;
        }
        if (totalGrowthTicks >= ticksNeededThree && totalGrowthTicks < ticksNeededFour)
        {
            return plantMaturity = 3;
        }

        if (totalGrowthTicks >= ticksNeededFour)
        {
            return plantMaturity = 4;
        }
        else return plantMaturity = 0;
    }

    public void SpreadSeeds(TileMono t)
    {
        if (plantMaturity == 2)
        {
            GridManager.Instance.SpreadSeeds(t.tileX, t.tileY, this);
        }
        
    }

    public void killPlant(TileMono t)
    {
       
    }
    
}
