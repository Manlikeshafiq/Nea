using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fox : MonoBehaviour
{
    public AnimalSO animalSO;
    public List<string> maturityNames = new List<string> { "Cub", "Young", "Young Adult", "Mature", "Old Ass" };
    public int currentState = 0;
    private int reproductionCooldown = 2160;
    //0 = idle - staying in its home tile (tile it spawned on)
    //1 = looking for water - A* pathfinding
    //2 = looking for food - A*
    //3 = reproduce if theres no cooldown and it can find mate

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
            reproductionCooldown--;
            
            if (animalSO.thirstLevel < 0)
            {
                t.RemoveAnimal(animalSO);

            }

            if (animalSO.hungerLevel < 0)
            {
                t.RemoveAnimal(animalSO);
            }
            animalSO.TicksNeeded();
            reproductionCooldown--;
            UpdateFoodAndWater(t);

            if (TimeTick.Instance.tick % 2 == 0)
            {
                UpdateState(t);

            }



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

        if (animalSO.thirstLevel < 50)
        {
            currentState = 1;
            PathfindToWater(t);
        }
        else if (animalSO.hungerLevel < 50)
        {
            currentState = 2;
            Debug.Log("finding food pls no crash");
            RabbitNearby(t);
        }
        else if (animalSO.animalMaturity >= 2 && reproductionCooldown <= 0)
        {
            currentState = 3;
            FindFoxForRepro(t);
        }
        else
        {
            currentState = 0;
            Idle(animalSO.spawnTile);
        }






    }


    public void FindFoxForRepro(TileMono t)
    {
        List<TileMono> tilesInRadius = GridManager.Instance.GetTilesWithinRadius(t.tileX, t.tileY, 6);


        TileMono nearestFoxTile = null;
        float minDistance = float.MaxValue;

        foreach (var tile in tilesInRadius)
        {
            foreach (var animal in tile.animalSlots)
            {
                if (animal.animalName == "Fox" && tile.tileSOData.tileBiome != "Water")
                {
                    float distance = AStarManager.instance.HexDistance(t, tile);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestFoxTile = tile;
                        Debug.Log("found fox" + nearestFoxTile + nearestFoxTile.tileX + nearestFoxTile.tileY + tile.tileX + t.tileY);
                    }

                }
            }
        }

        if (nearestFoxTile == null) { Debug.Log("null"); };

        if (nearestFoxTile != null)
        {
            Debug.Log("pathing");
            List<TileMono> path = AStarManager.instance.GeneratePath(t, nearestFoxTile);

            if (path != null && path.Count > 0)
            {
                Debug.Log("moving to find fox at " + nearestFoxTile + nearestFoxTile.tileX + nearestFoxTile.tileY);

                MoveToTile(nearestFoxTile);

                if (path[0].tileX == nearestFoxTile.tileY && path[0].tileY == nearestFoxTile.tileY)
                {
                    Reproduce(nearestFoxTile);
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
        reproductionCooldown = 8167;
    }
    public void PathfindToWater(TileMono currentTile)
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
                break;
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

    
    void MoveRandomlyThreeTiles(TileMono currentTile)
    {
        int i = 0;
        while (i < 4)
        {

            List<TileMono> neighbours = GridManager.Instance.GetNeighboursOfTIle(currentTile.tileX, currentTile.tileY).ToList();

            if (neighbours != null && neighbours.Count > 0)
            {
                int randomIndex = Random.Range(0, neighbours.Count);
                TileMono randomneighbours = neighbours[randomIndex];

                MoveToTile(randomneighbours);

                currentTile = randomneighbours;

                Debug.Log($"Moved randomly to tile: {randomneighbours.tileX}, {randomneighbours.tileY}");
            }
            else
            {

                i = 4;
            }




            Debug.Log("moving randomly");
            i++;
        }

    }



    //MERGE BOTH OF THESE IF TIME

    public void RabbitNearby(TileMono t)
    {


        List<TileMono> tilesInRadius = GridManager.Instance.GetTilesWithinRadius(t.tileX, t.tileY, 6);


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
                        Debug.Log("found rabbit" + nearestRabbitTile + nearestRabbitTile.tileX + nearestRabbitTile.tileY + tile.tileX + t.tileY);
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
                Debug.Log("moving to find rabbit at " + nearestRabbitTile + nearestRabbitTile.tileX + nearestRabbitTile.tileY);

                MoveToTile(nearestRabbitTile);

                if (path[0].tileX == nearestRabbitTile.tileY && path[0].tileY == nearestRabbitTile.tileY)
                {
                    AttackRabbit(nearestRabbitTile);
                }
            }

            else if (t.tileX == nearestRabbitTile.tileX && t.tileY == nearestRabbitTile.tileY)
            {
                AttackRabbit(nearestRabbitTile);
            }
        }
       
        else
        {
            Debug.Log("No rabbits found within radius.");
        }
    }


    public void AttackRabbit(TileMono tile)
    {
        Debug.Log("attacking");
        foreach (var animal in tile.animalSlots)
        {
            if (animal.animalName == "Rabbit")
            {
                int roll = Random.Range(0, 10);
                if (roll < animalSO.attackproficiency) 
                {
                    Debug.Log("fox kill rabbit");
                    animal.HP -= 100;
                    animalSO.attackproficiency += 0.5f;
                    animalSO.hungerLevel += 70;
                }
                else
                {
                    Debug.Log("roll fail");
                }
               
            }
        }
    }

    public void Idle(TileMono spawnTile)
    {

        TileMono currentTile = GetComponentInParent<TileMono>();
        if (currentTile != spawnTile)
        {
            Debug.Log("rabbit attempting move");
            MoveToTile(spawnTile);
        }
    }

    private void MoveToTile(TileMono targetTile)
    {
        TileMono currentTile = GetComponentInParent<TileMono>();


        List<TileMono> path = AStarManager.instance.GeneratePath(currentTile, targetTile);

        if (path != null && path.Count > 0)
        {
            TileMono nextTile = path[0];
            transform.SetParent(nextTile.transform);
            transform.position = nextTile.transform.position;
            Debug.Log($"Fox moved to tile: {nextTile.tileX}, {nextTile.tileY}");
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
                t.UpdateFoodAndWaterFox(animalSO);
            }
        }
    }

}
