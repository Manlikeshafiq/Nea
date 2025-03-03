using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileSize = 1.0f;

    public GameObject tilePrefab; //Add hexagonal sprite prefab 
    public TileSO[] tileTypes; //DO NOT CHANGE NAME ERRORRSSFOS OIFSCE 

    private TileMono[,] grid;

    void Start()
    {
        grid = new TileMono[gridWidth, gridHeight];
        GenerateGrid();
    }

    void GenerateGrid()
    {
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
        Dictionary<TileSO, float> weightedTilesDictionary = new Dictionary<TileSO, float>();  //KEY  = TILE. VALUE = WEIGHT

        foreach (var biome in tileTypes)
        {
            weightedTilesDictionary[biome] = biome.weight;   //add base weught
        }

        
        foreach (var neighbor in GetNeighborsOfTIle(x, y))   //change weight due to neighbours
        {
            if (neighbor != null && neighbor.tileSOData != null)
            {
                TileSO neighborTileData = neighbor.tileSOData;

                foreach (var biome in tileTypes)
                {
                    
                    foreach (var preference in biome.preferredNeighbors) 
                    {
                        if (preference.neighborTile == neighborTileData)
                        {
                            weightedTilesDictionary[biome] += preference.weightBoost; 

                        }
                    }
                }
            }
        }

        
        float totalWeight = 0f;
        foreach (var weightTilePair in weightedTilesDictionary) 
        {
            totalWeight += weightTilePair.Value;
        }

        float randomWeightValue = Random.Range(0, totalWeight);
        foreach (var pair in weightedTilesDictionary)
        {
            if (randomWeightValue < pair.Value)
                return pair.Key;                                       
                                                                        //Finds total weight. Random no between 0 and total weight. Iterates through and randomweight - by current tile weight until random weight is smaller than tile value where it then returns that tile
            randomWeightValue -= pair.Value;
        }

        
        return tileTypes[0]; // fallback (shouldn't happen INSHALLAH)
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
