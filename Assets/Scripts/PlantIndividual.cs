using Unity.VisualScripting;
using UnityEngine;

public class PlantTreeUpdates : MonoBehaviour
{
    public PlantSO plantso;


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
    }
}


