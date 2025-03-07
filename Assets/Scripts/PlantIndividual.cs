using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantTreeUpdates : MonoBehaviour
{
    public PlantSO plantso;
    private List<string> maturityNames = new List<string> { "Seedling", "Sapling", "Young", "Mature", "Old Ass" };


    public void Start()
    {
        TimeTick.OnTick += UpdatePlant;
    }

    void OnDestroy()
    { 
    }

    public void UpdatePlant()
    {
        plantso.plantMaturity += 1;
        plantso.hungerLevel -= 0.005;
        plantso.thirstLevel -= 0.198;

        plantso.TicksNeeded();

        if (plantso.plantMaturity >= 0 && plantso.plantMaturity < maturityNames.Count)
        {
            plantso.plantMaturityName = maturityNames[plantso.plantMaturity];
            
        }
        else
        {
            plantso.plantMaturityName = "Unknown"; 
            
        }

        plantso.nextPlantMaturityName = maturityNames[plantso.plantMaturity + 1];
    }
    public void SpreadPlant()
    {
        if (plantso.plantMaturity >= 2)
        {
        
        }
    }
}


