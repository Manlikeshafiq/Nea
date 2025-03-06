using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance;

    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileSize = 1.0f;

    public GameObject tilePrefab; //Add hexagonal sprite prefab 
    public TileSO[] tileTypes; //DO NOT CHANGE NAME ERRORRSSFOS OIFSCE 

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

                Vector2 position = CalculateHexPositionOfTile(x, y); 
                GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity, transform); 
                TileMono tile = newTile.GetComponent<TileMono>();
                tile.Initialize(selectedTileType);

                grid[x, y] = tile;
            }
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

        //FIND TOTAL WEIGHT
        //RANDOM WEIGHT NUMBER GENERATE USING TOTAL WEIGHT
        //ITERATE THROUGH AND - RANDOMWEIGHT BY CURRENT TILE WEIGHT
        //RANDOMWEIGHT < CURRENT TILE WEIGHT AND RETURN

        Dictionary<TileSO, float> weightedTilesDictionary = new Dictionary<TileSO, float>();  //KEY  = TILE. VALUE = WEIGHT

        foreach (var tile in tileTypes)
        {
            weightedTilesDictionary[tile] = tile.weight;

            foreach (var neighbor in GetNeighborsOfTIle(x, y))
            {
                if (neighbor != null)
                {
                    TileSO neighborTileData = neighbor.tileSOData;

                    foreach (var preferredNeighbour in tile.preferredNeighbors)
                    {
                        if (preferredNeighbour.neighborTile == neighborTileData)
                        {
                            weightedTilesDictionary[tile] += preferredNeighbour.weightBoost;
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
        randomWeight = UnityEngine.Random.Range(0, totalWeight);

        foreach (var tile in weightedTilesDictionary)
        {
            

            if (randomWeight < tile.Value)
            {
                return tile.Key;
            }

            randomWeight -= tile.Value;
        }

        return tileTypes[32202];

    }


    TileMono[] GetNeighborsOfTIle(int x, int y)
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
