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
    public int tileX = 3;
    public int tileY = 3;

    public int tileNo;
    public int plantID;


    int checkNumber;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(TileSO data, int x, int y)
    {
        //ALL DATA COMES FROM CLONE
        tileSOData = InstantiateTileSOData(data);
        name = $"Tile ({tileSOData.tileBiome})";
        tileNo = GridManager.Instance.tileNo;
        //ADD X,Y COORDS HERE?
        tileX = x; tileY = y;

        spriteRenderer.color = tileSOData.tileColour;


    }

    public void Start()
    {
        TimeTick.OnTick += Check1;




    }


    public void Check1()
    {

        List<PlantSO> tilesToRemove = new List<PlantSO>();

        foreach (PlantSO plant in plantSlots)
        {
            checkNumber = UnityEngine.Random.Range(0, plant.seedsPerYear);

            if (checkNumber == 4)
            {
                Debug.Log(checkNumber + "PLANTING ON " + this);
                plant.SpreadSeeds(this);
            }

            else
            {

            }
            if (plant.thirstLevel <= 0)
            {
                tilesToRemove.Add(plant);
            }
            if (plant.hungerLevel <= 0)
            {
                tilesToRemove.Add(plant);

            }
        }


        foreach (var tile in tilesToRemove)
        {
            RemovePlant(tile);
        }
    }



    public TileSO InstantiateTileSOData(TileSO data)
    {
        //INSTANTIATE SO FOR EACH TILE TO HAVE ITS OWN DATA

        TileSO newTile = ScriptableObject.CreateInstance<TileSO>();
        newTile.tileBiome = data.tileBiome;
        newTile.tileColour = data.tileColour;
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
            PlantSelectManager.Instance.TryPlantOnTile(this, PlantSelectManager.Instance.selectedPlant);
        }
        else if (PlantSelectManager.Instance.plantingMode != true)
        {
            UIManager.Instance.OpenTilePanel(this);

        }



    }






    public bool AddPlant(PlantSO selectedPlant)
    {
        if (plantSlots.Count < maxPlantSlots)
        {
            plantSlots.Add(selectedPlant);
            plantID++;



            GameObject plantObject = new GameObject(selectedPlant.plantName);
            plantObject.transform.SetParent(transform);

            if (selectedPlant.plantName == "Oak Tree")
            {
                PlantTreeUpdates plantData = plantObject.AddComponent<PlantTreeUpdates>();
                plantData.plantso = selectedPlant;
            }

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
        for (int i = 0; i < plantSlots.Count; i++)
        {
            if (plantSlots[i].plantID == plant.plantID)
            {
                Debug.Log("removed plant");

                plant.dead = true;
                plantSlots.RemoveAt(i);


            }
        }
    }

    public List<PlantSO> GetPlants()
    {
        return plantSlots;
    }


}

