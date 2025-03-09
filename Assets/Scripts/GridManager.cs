using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance;

    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileSize = 1.0f;
    public int tileNo = 0;


    public GameObject tilePrefab; //Add hexagonal sprite prefab 
    public TileSO[] tileTypes; //DO NOT CHANGE NAME ERRORRSSFOS OIFSCE 
    public PlantSO[] plants;

    public TileMono[,] grid;
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

    void Start()
    {
        grid = new TileMono[gridWidth, gridHeight];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        //GENERATE TILE USING WEIGHTS 
        //FIND LOCATION OF WHERE SAID TILE MUST GO
        //INSTANTIATE AND INITIALIZE

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                TileSO selectedTileType = GetWeightedTileType(x, y);
                Vector2 hexPositionOfTIle = CalculateHexPositionOfTile(x, y);
                GameObject newTile = Instantiate(tilePrefab, hexPositionOfTIle, Quaternion.identity, transform);
                TileMono tile = newTile.GetComponent<TileMono>();
                tile.Initialize(selectedTileType, x, y);

                grid[x, y] = tile;
                tileNo++;
                SpawnPlants(tile, x, y);
            }
        }
    }


    public void SpreadSeeds(int x, int y, PlantSO plantso)
    {
        Dictionary<TileMono, float> weightedTilesDictionary = new Dictionary<TileMono, float>();


        foreach (var neighbour in GetNeighboursOfTIle(x, y))
        {
            if (neighbour != null)
            {
                weightedTilesDictionary[neighbour] = 0.1f; // Base weight for all neighbors
                TileSO neighbourTileData = neighbour.tileSOData;

                foreach (var plant in plants)
                {
                    foreach (var preferredTile in plant.preferredTiles)
                    {
                        if (preferredTile.preferredTile.tileBiome == neighbourTileData.tileBiome)
                        {
                            weightedTilesDictionary[neighbour] += preferredTile.weightBoost;
                        }
                    }
                }
            }
        }

        float totalWeight = 0;
        float randomWeight = 0;

        foreach (var tile in weightedTilesDictionary)
        {
            totalWeight += tile.Value;
        }

        if (totalWeight == 0) return;

        randomWeight = UnityEngine.Random.Range(0f, totalWeight);

        float weightCheck = 0f;

        foreach (var tile in weightedTilesDictionary)
        {
            weightCheck += tile.Value;

            if (randomWeight <= weightCheck)
            {
                Debug.Log("planted" + tile.Key);
                PlantSelectManager.Instance.TryPlantOnTile(tile.Key, plantso);
                return;
            }
        }
    }




    public void SpawnPlants(TileMono tileMono, int x, int y)
    {
        Dictionary<PlantSO, float> weightAndPlants = new Dictionary<PlantSO, float>();  //KEY  = TILE. VALUE = WEIGHT


        //IF LESS PLANTS THAN MAX AND THERES A RANDOM CHANCE THAT NO PLANTS SPAWN ON TILE THEN:
        //LOOK THRU PLANTSOLIST
        //FIND PREFERRED TILES FOR THE PLANTSO
        //ADD WEIGHTBOOST
        //STORE WEIGHT




        tileMono = grid[x, y];
        int maxPlants = 5;
        int plantsPlanted = 0;
        float repeatChance = 0.3f;

        while (plantsPlanted < maxPlants && UnityEngine.Random.value < repeatChance)
        {
            foreach (var plant in plants)
            {

                bool prefersTile = false;
                float plantWeight = plant.weight;


                TileSO tileData = tileMono.tileSOData;


                foreach (var preferredTile in plant.preferredTiles)
                {
                    if (preferredTile.preferredTile.tileBiome == tileData.tileBiome)
                    {
                        plantWeight += preferredTile.weightBoost;
                        prefersTile = true;


                    }
                }

                if (prefersTile)
                {
                    weightAndPlants[plant] = plantWeight;
                }



            }



            float totalWeight = 0;
            foreach (var plant in weightAndPlants)
            {
                totalWeight += plant.Value;
            }

            float randomWeight = UnityEngine.Random.Range(0f, totalWeight);

            float cumulativeWeight = 0f;
            foreach (var plant in weightAndPlants)
            {
                cumulativeWeight += plant.Value;

                if (randomWeight <= cumulativeWeight)
                {
                    PlantSelectManager.Instance.TryPlantOnTile(tileMono, plant.Key);
                    plantsPlanted++;

                }
            }
            repeatChance *= 0.5f;

        }
    }

    public Vector2 GetWorldBounds()
    {
        float gridWidth = (this.gridWidth - 1) * tileSize * 0.75f;
        float gridHeight = (this.gridHeight - 1) * tileSize * Mathf.Sqrt(3) / 2;

        return new Vector2(gridWidth, gridHeight);
    }

    TileSO GetWeightedTileType(int x, int y)
    {

        //ADD WEIGHT TO ALL TILES
        //GET NEIGHBOURS
        //NULL CHECK
        //GO THRU PREFERRED NEIGHBOURS
        //ADD WEIGHTBOOST TO TILE



        Dictionary<TileSO, float> weightedTilesDictionary = new Dictionary<TileSO, float>();  //KEY  = TILE. VALUE = WEIGHT

        foreach (var tile in tileTypes)
        {
            weightedTilesDictionary[tile] = tile.weight;

            foreach (var neighbour in GetNeighboursOfTIle(x, y))
            {

                if (neighbour != null)
                {

                    TileSO neighbourTileData = neighbour.tileSOData;


                    foreach (var preferredNeighbour in tile.preferredNeighbors)
                    {

                        if (preferredNeighbour.neighborTile.tileBiome == neighbourTileData.tileBiome)
                        {

                            weightedTilesDictionary[tile] += preferredNeighbour.weightBoost;
                        }
                    }
                }
            }
        }
        //EG
        //[Grass, 10]
        //[Savannah, 5]
        //Total = 15
        //if random = 10 then grass chosen, if random = 11 then savannah chosen


        float totalweight = 0;

        /*
        foreach (var value in weightedTilesDictionary)
        {
            if (value.Value < 0)
            {
                weightedTilesDictionary.Remove(value.Key);
            }

            totalweight += value.Value;

        }

        float randomweight = Random.Range(0, 1f);
        */
        List<TileSO> tilesToRemove = new List<TileSO>();
        foreach (var value in weightedTilesDictionary)
        {
            if (value.Value < 0)
            {
                tilesToRemove.Add(value.Key);
            }
            else
            {
                totalweight += value.Value;
            }
        }


        foreach (var tile in tilesToRemove)
        {
            weightedTilesDictionary.Remove(tile);
        }


        float randomweight = Random.Range(0f, 1f);
        foreach (var tile in weightedTilesDictionary.Keys.ToList())
        {
            weightedTilesDictionary[tile] /= totalweight;
        }

        float weightCheck = 0f;

        foreach (var value in weightedTilesDictionary)
        {

            weightCheck += value.Value;

            if (randomweight <= weightCheck)
            {

                return value.Key;
            }

        }



        Debug.Log("returned");
        return tileTypes[0];

    }


    TileMono[] GetNeighboursOfTIle(int x, int y)
    {
        List<TileMono> neighborsList = new List<TileMono>();


        int[][] directionsToNeighbourTile = new int[][]
        {
            //         X    Y
            new int[] { 0,  1 },
            new int[] { 0, -1 }, // V


            new int[] { 1,  0 },
            new int[] { -1, 0 }, // H
           
            

            new int[] { 1, -1 },
            new int[] { -1, 1 } // D
        };

        foreach (var direction in directionsToNeighbourTile)
        {
            int neighbourX = x + direction[0];
            int neighbourY = y + direction[1];
            if (neighbourX >= 0 && neighbourX < gridWidth
                && neighbourY >= 0 && neighbourY < gridHeight)
            {
                neighborsList.Add(grid[neighbourX, neighbourY]);
            }
        }

        return neighborsList.ToArray();
    }

    Vector2 CalculateHexPositionOfTile(int x, int y)
    {
        float xOffset = tileSize * 0.75f;
        float yOffset = tileSize * Mathf.Sqrt(3) / 2f;

        float xPos = x * xOffset;
        float yPos = y * yOffset;


        if (x % 2 == 1)
        {
            yPos += yOffset / 2f;
        }

        return new Vector2(xPos, yPos);
    }
}