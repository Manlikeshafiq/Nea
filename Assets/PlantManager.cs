using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;

    public PlantSO selectedPlant;
    public List<PlantSO> plantSOList;

    public TileSO TileSOData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SelectPlant(int planSOListtIndex)
    {
        if (planSOListtIndex >= 0 && planSOListtIndex < plantSOList.Count)
        {
            selectedPlant = plantSOList[planSOListtIndex];


            Debug.Log("Selected Plant: " + selectedPlant.plantName);
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    public void TryPlantOnTile(TileMono t)
    {
        if (t != null && selectedPlant != null)
        {
            bool tilePlantSuccess = t.AddPlant(selectedPlant);
            if (tilePlantSuccess)
            {
                Debug.Log($"Planted {selectedPlant.plantName} on tile {t.gameObject.name}");
            }
        }
    }




}