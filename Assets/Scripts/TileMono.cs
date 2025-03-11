using JetBrains.Annotations;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public List<AnimalSO> animalSlots = new List<AnimalSO>();


    public int maxPlantSlots = 5;
    public int maxAnimalSlots = 3;
    public int tileX = 3;
    public int tileY = 3;

    public int tileNo;
    public int plantID;


    public float gScore; // Cost from start to this tile
    public float hScore; // Heuristic cost to reach the end
    public TileMono cameFrom; // Parent tile in the path

    // Calculate the total cost (FScore)
    public float FScore() => gScore + hScore;

    int checkNumber;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(TileSO data, int x, int y)
    {
        //ALL DATA COMES FROM CLONE
        tileSOData = InstantiateTileSOData(data);
        name = $"Tile ({tileSOData.tileBiome}, {x}. {y})";
        tileNo = GridManager.Instance.tileNo;
        //ADD X,Y COORDS HERE?
        tileX = x; tileY = y;

        spriteRenderer.color = tileSOData.tileColor;


    }

    public void Start()
    {
        TimeTick.OnTick += Check1;




    }


    public void Check1()
    {

       

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
            if (plant.HP <= 0)
            {
                Debug.Log(plant.plantName + "died");
                RemovePlant(plant);
            }
        }


        
    }
    public bool AddAnimal(AnimalSO animal)
    {
        if (animalSlots.Count < maxAnimalSlots)
        {
            GameObject animalObject = new GameObject(animal.animalName);
            animalObject.transform.SetParent(transform);

            if (animal.animalName == "Rabbit")
            {
                RabbitScripts animalData = animalObject.AddComponent<RabbitScripts>();
                animalData.animalSO = animal;
            }

           if  (animal.animalName == "Fox")
            {
                Fox animalData = animalObject.AddComponent<Fox>();
                animalData.animalSO = animal;
            }


            animalSlots.Add(animal);
            return true;
        }
        return false;
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
            PlantSelectManager.Instance.TryPlantOnTile(this, PlantSelectManager.Instance.selectedPlant);
        }
        else if (PlantSelectManager.Instance.plantingMode != true)
        {
            UIManager.Instance.OpenTilePanel(this);
            
        }

        

    }


    public void UpdateFoodAndWaterRabbit(AnimalSO animal)
    {
        foreach (PlantSO plant in plantSlots)
        {
            foreach (AnimalSO.EdiblePlants EP in animal.ediblePlants)
            {
                if (EP.ediblePlant.plantName == plant.plantName)
                {
                    animal.hungerLevel += 10;
                    plant.HP -= 40;
                    Debug.Log($"{animal.animalName} ate {plant.plantName} on tile {tileX},{tileY}");

                    if (plant.HP <= 0)
                    {
                        RemovePlant(plant);
                    }
                    break;
                }
            }
        }

        
        foreach (TileMono neighbour in GridManager.Instance.GetNeighboursOfTIle(tileX, tileY))
        {
            if (neighbour.tileSOData.tileBiome == "Water") 
            {
                animal.thirstLevel = 100;
                Debug.Log($"{animal.animalName} drank water from nearby tile {neighbour.tileX},{neighbour.tileY}");
                return;
            }
        }
    }

    public void UpdateFoodAndWaterFox(AnimalSO animal)
    {
      

        foreach (TileMono neighbour in GridManager.Instance.GetNeighboursOfTIle(tileX, tileY))
        {
            if (neighbour.tileSOData.tileBiome == "Water")
            {
                animal.thirstLevel = 100;
                Debug.Log($"{animal.animalName} drank water from nearby tile {neighbour.tileX},{neighbour.tileY}");
                return;
            }
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

            if (selectedPlant.plantName == "Grass")
            {
                Grass plantData = plantObject.AddComponent<Grass>();
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
                Debug.Log("removed plant" + plant.plantName);
               
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

        
