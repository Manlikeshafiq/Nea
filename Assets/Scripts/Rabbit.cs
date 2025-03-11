using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using static AnimalSO;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class RabbitScripts : MonoBehaviour
{
   

    public AnimalSO animalSO;
    public List<string> maturityNames = new List<string> { "Cub", "Young", "Young Adult", "Mature", "Old Ass" };
    public int currentState = 0;
    public int reproductionCooldown = 720;
    //0 = idle - staying in its home tile (tile it spawned on)
    //1 = looking for water - A* pathfinding
    //2 = looking for food - A*
    //3 = running - 

    public void Start()
    {
        TimeTick.OnTick += UpdateAnimal;
    }

    void OnDestroy()
    {

    }

    public void UpdateAnimal()
    {
        TileMono t = GetComponentInParent<TileMono>();
        if (animalSO.dead == false)
        {
            animalSO.totalGrowthTicks += 1;
            animalSO.hungerLevel -= 1.4f;
            animalSO.thirstLevel -= 2;


            UpdateFoodAndWater(t);
            UpdateState(t);
            animalSO.TicksNeeded();
            reproductionCooldown--;
            if (animalSO.animalMaturity >= 0 && animalSO.animalMaturity < maturityNames.Count)
            {
                animalSO.animalMaturityName = maturityNames[animalSO.animalMaturity];

            }
            else
            {
                animalSO.animalMaturityName = "Unknown";

            }

            if (animalSO.animalMaturity + 1 < maturityNames.Count) { animalSO.nextAnimalMaturityName = maturityNames[animalSO.animalMaturity + 1]; }
        }
    }

    void UpdateState(TileMono t)
    {
        if (FoxNearby(t))
        {
            currentState = 3;
            CheckForSafeTiles(t);
           

        }
        else
        {
            if (animalSO.thirstLevel < 50)
            {
                currentState = 1;
                PathfindToWater(t);
            }
            else if (animalSO.hungerLevel < 50)
            {
                currentState = 2;
                PathfindToFood(t);
            }
            else if (animalSO.animalMaturity >= 2 && reproductionCooldown <= 0)
            {
                currentState = 4;
                FindRabbitForRepro(t);
            }
            else
            {
                currentState = 0;
                Idle(animalSO.spawnTile);
            }
        }
       
        
    }

    private void PathfindToWater(TileMono currentTile)
    {
        List<TileMono> tilesInRadius = GridManager.Instance.GetTilesWithinRadius(currentTile.tileX, currentTile.tileY, 3);

        TileMono nearestWaterTile = null;
        float minDistance = float.MaxValue;

        foreach (var tile in tilesInRadius)
        {
            if (tile.tileSOData.tileBiome == "Water")
            {
                float distance = AStarManager.instance.HexDistance(currentTile, tile);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestWaterTile = tile;
                }
            }
        }

        if (nearestWaterTile != null)
        {
            List<TileMono> path = AStarManager.instance.GeneratePath(currentTile, nearestWaterTile);

            if (path != null && path.Count > 0)
            {
                Debug.Log("moving to find water at " + nearestWaterTile + nearestWaterTile.tileX + nearestWaterTile.tileY);

                MoveToTile(nearestWaterTile);
            }
        }
        else
        {
            MoveRandomlyThreeTiles(currentTile);
        }
    }

    private void PathfindToFood(TileMono currentTile)
    {
        List<TileMono> tilesInRadius = GridManager.Instance.GetTilesWithinRadius(currentTile.tileX, currentTile.tileY, 3);

        TileMono nearestFoodTile = null;
        float minDistance = float.MaxValue;

        foreach (var tile in tilesInRadius)
        {
            for (int i = 0; i < tile.plantSlots.Count; i++)
            {
                if (tile.plantSlots[i].plantName == "Grass")
                {
                    float distance = AStarManager.instance.HexDistance(currentTile, tile);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestFoodTile = tile;
                    }
                }
            }
        }

        if (nearestFoodTile != null)
        {
            List<TileMono> path = AStarManager.instance.GeneratePath(currentTile, nearestFoodTile);

            if (path != null && path.Count > 0)
            {
                Debug.Log("moving to find food at " + nearestFoodTile + nearestFoodTile.tileX + nearestFoodTile.tileY);

                MoveToTile(nearestFoodTile);
            }
        }
        else
        {
            MoveRandomlyThreeTiles(currentTile);
        }
    }

    void MoveRandomlyThreeTiles(TileMono currentTile)
    {
        int i = 0;
        while (i < 4)
        {
            Debug.Log("moving randomly");
            CheckForSafeTiles(currentTile);
            i++;
        }
       
    }

    public void FindRabbitForRepro(TileMono t)
    {
        List<TileMono> tilesInRadius = GridManager.Instance.GetTilesWithinRadius(t.tileX, t.tileY, 3);


        TileMono nearestRabbitTile = null;
        float minDistance = float.MaxValue;

        foreach (var tile in tilesInRadius)
        {
            foreach (var animal in tile.animalSlots)
            {
                if (animal.animalName == "Rabbit" && tile.tileSOData.tileBiome != "Water")
                {
                    float distance = AStarManager.instance.HexDistance(t, tile);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestRabbitTile = tile;
                        Debug.Log("found repro Rabbit" + nearestRabbitTile + nearestRabbitTile.tileX + nearestRabbitTile.tileY + tile.tileX + t.tileY);
                    }

                }
            }
        }

        if (nearestRabbitTile == null) { Debug.Log("null"); };

        if (nearestRabbitTile != null)
        {
            Debug.Log("pathing");
            List<TileMono> path = AStarManager.instance.GeneratePath(t, nearestRabbitTile);

            if (path != null && path.Count > 0)
            {
                Debug.Log("moving to find repro rabbit at " + nearestRabbitTile + nearestRabbitTile.tileX + nearestRabbitTile.tileY);

                MoveToTile(nearestRabbitTile);

                if (path[0].tileX == nearestRabbitTile.tileY && path[0].tileY == nearestRabbitTile.tileY)
                {
                    Reproduce(nearestRabbitTile);
                }
            }


        }

        else
        {
            Debug.Log("No rabbits found within radius.");
        }
    }

    public void Reproduce(TileMono t)
    {
        GridManager.Instance.SpawnAnimals(t, t.tileX, t.tileY);
        animalSO.spawnTile = t;
        reproductionCooldown = 720;

    }

    //MERGE BOTH OF THESE IF TIME

    bool FoxNearby(TileMono t)
    {


        foreach (TileMono neighbour in GridManager.Instance.GetNeighboursOfTIle(t.tileX, t.tileY))
        {

            for (int i = 0; i < neighbour.animalSlots.Count; i++)
            {
                if (neighbour.animalSlots[i].animalName == "Fox" ||
                    t.animalSlots[i].animalName == "Fox")
                {
                    Debug.Log("neighbour" + neighbour + neighbour.tileX + neighbour.tileY + "has fox");
                    return true;
                }
            }


        }

        for (int i = 0; i < t.animalSlots.Count; i++)
        {
            if (t.animalSlots[i].animalName == "Fox"
                )
            {
                Debug.Log("neighbour" + t + t.tileX + t.tileY + "has fox");
                return true;
            }
        }


    
        return false;
    }

    public void CheckForSafeTiles(TileMono t)
    {
        List<TileMono> safeTiles = new List<TileMono>();
        foreach (TileMono neighbour in GridManager.Instance.GetNeighboursOfTIle(t.tileX, t.tileY))
        {
            bool foxInTile = false;
            bool foxInSpawn = false;

            for (int i = 0; i < t.animalSlots.Count; i++)
            {
                if (t.animalSlots[i].animalName == "Fox")
                {
                    foxInSpawn = true;

                    Debug.Log("neighbour" + t + t.tileX + t.tileY + "has fox");
                }
            }


            if (neighbour.animalSlots != null)
            {
                for (int i = 0; i < neighbour.animalSlots.Count; i++) 
                {
                    if (neighbour.animalSlots[i].animalName == "Fox")
                    {
                        foxInTile = true;
                    }
                }
            }   
            if (neighbour.tileSOData.tileBiome != "Water" && (foxInTile == false || foxInSpawn == true ))
            {
                if (t != neighbour)
                {

                    safeTiles.Add(neighbour);
                }
                else
                {
                    Debug.Log(t, neighbour);
                }
            }

        }

        
        MoveToSafeTile(safeTiles);
    }    

    public void MoveToSafeTile(List<TileMono> safeTiles)
    {
        int roll = Random.Range(0, 10);
        if (safeTiles.Count > animalSO.attackproficiency)
        {
            if (roll < 5)
            {
                TileMono newTile = safeTiles[Random.Range(0, safeTiles.Count)];
                transform.SetParent(newTile.transform);
                transform.position = newTile.transform.position;
                Debug.Log("Rabbit moved to a safer tile" + newTile);
                animalSO.attackproficiency -= 0.3f;
            }
        }
        else
        {
            Debug.Log("No safe tiles to move to.");
        }
    }

    public void Idle(TileMono spawnTile)
    {
        if (spawnTile != null && spawnTile.animalSlots != null)
        {
            foreach (var animal in spawnTile.animalSlots)
            {
                if (animal.animalName == "Fox")
                {
                    Debug.Log("Fox detected on spawn tile! Changing spawn tile to current location.");
                    spawnTile = GetComponentInParent<TileMono>();
                    return;
                }
            }
        }

        TileMono currentTile = GetComponentInParent<TileMono>();
        if (currentTile != spawnTile)
        {
            Debug.Log("fox attempting move");
            MoveToTile(spawnTile);
        }
    }

    private void MoveToTile(TileMono targetTile)
    {
        // Get the current tile
        TileMono currentTile = GetComponentInParent<TileMono>();


        // Use A* to find the path
        List<TileMono> path = AStarManager.instance.GeneratePath(currentTile, targetTile);

        if (path != null && path.Count > 0)
        {
            // Move the rabbit to the next tile in the path
            TileMono nextTile = path[0];
            transform.SetParent(nextTile.transform);
            transform.position = nextTile.transform.position;
            Debug.Log($"Rabbit moved to tile: {nextTile.tileX}, {nextTile.tileY}");
        }
       
        else
        {
            Debug.Log("No path found to the target tile.");
        }

    }
    public void UpdateFoodAndWater(TileMono t)
    {
        if (TimeTick.Instance.dayOrNight == true) { }
        else
        {
           
            if (t != null)
            {
                t.UpdateFoodAndWaterRabbit(animalSO);
            }
        }
    }



}




