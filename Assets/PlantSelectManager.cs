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

        return selectedPlantInstantiated;
    }

    public void SelectPlant(int planSOListtIndex)
    {
        if (planSOListtIndex >= 0 && planSOListtIndex < plantSOList.Count)
        {
            selectedPlant = ClonePlantSO(plantSOList[planSOListtIndex]);
            plantingMode = true;

            Debug.Log("Selected Plant: " + selectedPlant.plantName);
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    public void TryPlantOnTile(TileMono t, PlantSO selectedPlant)
    {
        if (t != null && selectedPlant != null)

        {
            
            tilePlanted = t.AddPlant(ClonePlantSO(selectedPlant));
            if (tilePlanted)
            {
                
                Debug.Log($"Planted {selectedPlant.plantName} on tile {t.gameObject.name}");
            }
        }
    }

    public void SwitchPlantingModeFalse()
    {
        plantingMode = false;
    }


}