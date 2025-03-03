using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Plants/PlantSO")]
public class PlantSO : ScriptableObject
{
    public string plantName;
    public string plantFamily;
    public Sprite plantSprite;

    public int HP;
    public int hungerLevel;
    public int thirstLevel;



    public float totalGrowthTicks;
    public float ticksNeededOne;
    public float ticksNeededTwo;
    public float ticksNeededThree;
    public int plantMaturity;
    
}
