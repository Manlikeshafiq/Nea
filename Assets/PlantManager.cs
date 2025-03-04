using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;

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

    public void SelectPlant(int planSOListtIndex)

        //CHECK FOR NULL
        //ADD ALL PLANTS IN INSPECTOR
        //MAKE ENUM FOR PLANTS???
        //CHANGE THE INDEX FOR THE BUTTON WHICH CORRESPONDS TO PLANT IN INSPECTOR. EG 0 = TREE
        //ADD PLANT TO LIST
    {
        if (planSOListtIndex >= 0 && planSOListtIndex < plantSOList.Count)
        {
            selectedPlant = plantSOList[planSOListtIndex];
            plantingMode = true;

            Debug.Log("Selected Plant: " + selectedPlant.plantName);
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    public void TryPlantOnTile(TileClones tile)
    {
        //CHECK FOR NULL
        //REFERENCE TILEMONO ADDPLANT TO FIND IF TRUE
        

        if (tile != null && selectedPlant != null)

        {
            tilePlanted = tile.AddPlant(selectedPlant);
            if (tilePlanted)
            {
                
                Debug.Log($"Planted {selectedPlant.plantName} on tile {tile.gameObject.name}");
            }
        }
    }

    public void SwitchPlantingModeFalse()
    {
        plantingMode = false;
    }


}