
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantManager : MonoBehaviour
{
    public PlantSO plantSO;
    protected List<string> maturityNames;
 


    public virtual void Start()
    {
        TimeTick.OnTick += UpdatePlant;
    }

    public virtual void OnDestroy()
    {
        TimeTick.OnTick -= UpdatePlant;
    }

    public virtual void UpdatePlant()
    {

        
        if (!plantSO.dead)
        {
            plantSO.totalGrowthTicks++;
            plantSO.hungerLevel -= plantSO.hungerReductionRate;
            plantSO.thirstLevel -= plantSO.thirstReductionRate;

            UpdateFoodAndWater();
            plantSO.TicksNeeded();
            UpdateMaturityNames();
            ApplyAgeHealthPenalty();
        }
    }

    protected void UpdateMaturityNames()
    {
        if (plantSO.plantMaturity >= 0 && plantSO.plantMaturity < maturityNames.Count)
        {
            plantSO.plantMaturityName = maturityNames[plantSO.plantMaturity];
        }
        else
        {
            plantSO.plantMaturityName = "Unknown";
        }

        if (plantSO.plantMaturity + 1 < maturityNames.Count)
        {
            plantSO.nextPlantMaturityName = maturityNames[plantSO.plantMaturity + 1];
        }
    }

    protected void ApplyAgeHealthPenalty()
    {
        if (plantSO.plantMaturity >= 4)
        {
            plantSO.HP -= 4;
        }
    }

    protected abstract void UpdateFoodAndWater();
}


