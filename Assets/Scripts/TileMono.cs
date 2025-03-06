using JetBrains.Annotations;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TileMono : MonoBehaviour
{
    public TileSO tileSOData;  

    private SpriteRenderer spriteRenderer;


    public List<PlantSO> plantSlots = new List<PlantSO>();
    public int maxPlantSlots = 5;
    public int maxAnimalSlots = 3;

    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(TileSO data)
    {
        //ALL DATA COMES FROM CLONE
        tileSOData = InstantiateTileSOData(data);
        name = $"Tile ({tileSOData.tileBiome})";

        //ADD X,Y COORDS HERE?

        spriteRenderer.color = tileSOData.tileColor;


    }


    public TileSO InstantiateTileSOData(TileSO data)
    {
        //INSTANTIATE SO FOR EACH TILE TO HAVE ITS OWN DATA

        TileSO newTile = ScriptableObject.CreateInstance<TileSO>();
        newTile.tileBiome = data.tileBiome;
        newTile.tileColor = data.tileColor;
        newTile.weight = data.weight;
        newTile.waterLeve = data.waterLeve;
        newTile.sunlightLevel = data.sunlightLevel;
        newTile.fertilityLevel = data.fertilityLevel;
        return newTile;
    }




    private void OnMouseDown()  //Temporary Solution (?)
    {
        if (PlantSelectManager.Instance != null && PlantSelectManager.Instance.plantingMode)
        {
            PlantSelectManager.Instance.TryPlantOnTile(this);
        }
        else if (PlantSelectManager.Instance.plantingMode != true)
        {
            UIManager.Instance.OpenTilePanel(this);
            for(int i = 0; i < plantSlots.Count; i++) { Debug.Log(plantSlots[i].ToString()); }
            
        }

        

    }

    




    public bool AddPlant(PlantSO selectedPlant)
    {
        if (plantSlots.Count < maxPlantSlots)
        {
            plantSlots.Add(selectedPlant);



            GameObject plantObject = new GameObject(selectedPlant.plantName);
            plantObject.transform.SetParent(transform);

            if (selectedPlant.plantName == "Oak Tree")
            {
                PlantTreeUpdates plantData = plantObject.AddComponent<PlantTreeUpdates>();
                plantData.plantso = selectedPlant;
            }

            Debug.Log($"Added {selectedPlant.plantName}  {gameObject.name}");
            return true;
        }
        else
        {
            Debug.Log("no more tilees available");
            return false;
        } 
        }

    
    public void RemovePlant(PlantSO plant, int i)
    {
        if (plantSlots[i] != null)
        {
            plantSlots[i] = null;
            Debug.Log($"Removed {plant.plantName}  {gameObject.name}");
        }
    }

    public List<PlantSO> GetPlants()
    {
        return plantSlots;
    }


}

        
