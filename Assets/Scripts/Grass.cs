using System.Collections.Generic;
using UnityEngine;

public class Grass : PlantManager
{
    public override void Start()
    {
        maturityNames = new List<string> { "Sprout", "Seedling", "Mature", "Tall", "Old Ass" };
        plantSO.hungerReductionRate = 0.005f;
        plantSO.thirstReductionRate = 0.198f;
        base.Start();
    }

    protected override void UpdateFoodAndWater()
    {
        TileMono t = GetComponentInParent<TileMono>();
        int oneDay = TimeTick.Instance.tick % 24;
        int quarterDay = TimeTick.Instance.tick % 6;

        if (oneDay == 0)
        {
            int randomChanceWater = Random.Range(0, t.tileSOData.waterLeve);
            if (randomChanceWater < t.tileSOData.waterLeve / 2)
            {
                plantSO.thirstLevel += 0.5f;
                plantSO.HP += 0.1f;
            }
        }

        if (quarterDay == 0 && TimeTick.Instance.dayOrNight)
        {
            int randomChanceFood = Random.Range(0, t.tileSOData.fertilityLevel);
            if (randomChanceFood < t.tileSOData.fertilityLevel / 2)
            {
                plantSO.hungerLevel += 0.2f;
                plantSO.HP += 0.1f;
            }
        }
    }
}