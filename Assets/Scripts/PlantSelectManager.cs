using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlantSelectManager : MonoBehaviour
{
    public static PlantSelectManager Instance;

    public PlantSO selectedPlant;
    public List<PlantSO> plantSOList;
    public bool tilePlanted;
    public bool plantingMode = false;
    public TileSO TileSOData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PlantSO ClonePlantSO(PlantSO data)
    {
        PlantSO selectedPlantInstantiated = ScriptableObject.CreateInstance<PlantSO>();
        selectedPlantInstantiated.plantName = data.plantName;
        selectedPlantInstantiated.HP = data.HP;
        selectedPlantInstantiated.hungerLevel = data.hungerLevel;
        selectedPlantInstantiated.thirstLevel = data.thirstLevel;
        selectedPlantInstantiated.plantMaturity = data.plantMaturity;
        selectedPlantInstantiated.ticksNeededOne = data.ticksNeededOne;
        selectedPlantInstantiated.ticksNeededTwo = data.ticksNeededTwo;
        selectedPlantInstantiated.ticksNeededThree = data.ticksNeededThree;
        selectedPlantInstantiated.ticksNeededFour = data.ticksNeededFour;
        selectedPlantInstantiated.preferredTiles = data.preferredTiles;
        selectedPlantInstantiated.seedsPerYear = data.seedsPerYear;
        selectedPlantInstantiated.plantID = data.plantID;
        selectedPlantInstantiated.dead = data.dead; 

        return selectedPlantInstantiated;
    }

    public void SelectPlant(int planSOListtIndex)
    {
        if (planSOListtIndex >= 0 && planSOListtIndex < plantSOList.Count)
        {
            selectedPlant = ClonePlantSO(plantSOList[planSOListtIndex]);
            plantingMode = true;

        }
       
    }

    public void TryPlantOnTile(TileMono t, PlantSO selectedPlant)
    {
        if (t != null && selectedPlant != null)

        {
            
            tilePlanted = t.AddPlant(ClonePlantSO(selectedPlant));
            selectedPlant.plantID = t.plantID;
            if (tilePlanted)
            {
                
            }
        }
    }

    public void SwitchPlantingModeFalse()
    {
        plantingMode = false;
    }


}