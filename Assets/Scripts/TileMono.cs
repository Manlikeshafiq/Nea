using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMono : MonoBehaviour
{
    public TileSO tileSOData;  

    private SpriteRenderer spriteRenderer;


    private List<PlantSO> plantSlots = new List<PlantSO>();
    public int maxPlantSlots = 5;
    public int maxAnimalSlots = 3;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(TileSO data)
    {
        tileSOData = data;
        name = $"Tile ({tileSOData.tileBiome})";

        //ADD X,Y COORDS HERE?

        spriteRenderer.color = tileSOData.tileColor;


    }


    private void OnMouseDown()  //Temporary Solution (?)
    {
        if (PlantManager.Instance != null && PlantManager.Instance.plantingMode)
        {
            PlantManager.Instance.TryPlantOnTile(this);
        }
        else if (PlantManager.Instance.plantingMode != true)
        {
            TileUI();
        }
    }

    private void TileUI()
    {
        UIManager.Instance.OpenTilePanel();
        UIManager.Instance.tileTitleText.text = tileSOData.tileBiome ;
        UIManager.Instance.tileWeight.text = tileSOData.weight.ToString() ;
        UIManager.Instance.tileWater.text = tileSOData.waterLeve.ToString() ;
        UIManager.Instance.tileSunlight.text = tileSOData.sunlightLevel.ToString() ;
        UIManager.Instance.tileMaturity.text = tileSOData.fertilityLevel.ToString() ;


        tileSOData.weight = (float)Convert.ToDouble(UIManager.Instance.tileWeight.text);
        tileSOData.waterLeve = Convert.ToInt32(UIManager.Instance.tileWater.text);
        tileSOData.sunlightLevel = Convert.ToInt32(UIManager.Instance.tileSunlight.text);

        tileSOData.fertilityLevel = Convert.ToInt32(UIManager.Instance.tileMaturity.text);


    }




    public bool AddPlant(PlantSO selectedPlant)
    {
        if (plantSlots.Count < maxPlantSlots)
        {
            plantSlots.Add(selectedPlant);
            Debug.Log($"Added {selectedPlant.plantName}  {gameObject.name}");
            return true;
        }
        else
        {
            Debug.Log("no more tilees available");
            return false;
        } 
        }

    public void RemovePlant(PlantSO plant)
    {
        if (plantSlots.Contains(plant))
        {
            plantSlots.Remove(plant);
            Debug.Log($"Removed {plant.plantName}  {gameObject.name}");
        }
    }

    public List<PlantSO> GetPlants()
    {
        return plantSlots;
    }
}

        
