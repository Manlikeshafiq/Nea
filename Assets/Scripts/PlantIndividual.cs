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

        if (plantso.dead == false)
        {
            plantso.totalGrowthTicks += 1;
            plantso.hungerLevel -= 0.005f;
            plantso.thirstLevel -= 0.198f;


            UpdateFoodAndWater();

            plantso.TicksNeeded();

            if (plantso.plantMaturity >= 0 && plantso.plantMaturity < maturityNames.Count)
            {
                plantso.plantMaturityName = maturityNames[plantso.plantMaturity];

            }
            else
            {
                plantso.plantMaturityName = "Unknown";

            }

            if (plantso.plantMaturity + 1 < maturityNames.Count) { plantso.nextPlantMaturityName = maturityNames[plantso.plantMaturity + 1]; }
        }
    }


    public void UpdateFoodAndWater()
    {

        

            TileMono t = GetComponentInParent<TileMono>();
            int oneDay = (TimeTick.Instance.tick % 24);
            int quarterDay = (TimeTick.Instance.tick % 6);
            if (oneDay == 0)
            {
                Debug.Log("day");
                int randomChanceWater = Random.Range(0, t.tileSOData.waterLeve);
                if (randomChanceWater < t.tileSOData.waterLeve / 2)
                {
                    Debug.Log($"{t.ToString()} has gotten + 0.5 water");

                    plantso.thirstLevel += 0.5f;
                }


            }

            if (quarterDay == 0)
            {
                if (TimeTick.Instance.dayOrNight)
                {
                    int randomChanceFood = Random.RandomRange(0, t.tileSOData.fertilityLevel);
                    if (randomChanceFood < t.tileSOData.fertilityLevel / 2)
                    {
                        Debug.Log($"{t.ToString()} has gotten + 0.2 food");
                        plantso.thirstLevel += 0.2f;
                    }
                }
            }
        
    }

    
}


