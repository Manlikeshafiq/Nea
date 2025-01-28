using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float tileSize = 1.0f;

    public GameObject tilePrefab; // Prefab with the Tile script attached
    public TileSO[] tileTypes; // Array of Tile Scriptable Objects

    private Tile[,] grid;

    void Start()
    {
        grid = new Tile[width, height];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Determine the tile type for this position
                TileSO selectedTileType = GetWeightedTileType(x, y);

                // Instantiate the tile
                Vector2 position = CalculateHexPosition(x, y);
                GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                Tile tile = newTile.GetComponent<Tile>();
                tile.Initialize(selectedTileType);

                // Store the tile in the grid
                grid[x, y] = tile;
            }
        }
    }

    public Vector2 GetWorldBounds()
    {
        float gridWidth = (width - 1) * tileSize * 0.75f; // Horizontal stagger
        float gridHeight = (height - 1) * tileSize * Mathf.Sqrt(3) / 2; // Vertical spacing

        return new Vector2(gridWidth, gridHeight);
    }

    TileSO GetWeightedTileType(int x, int y)
    {
        Dictionary<TileSO, float> weightedTiles = new Dictionary<TileSO, float>();

        // Initialize weights based on the base weight
        foreach (var tileType in tileTypes)
        {
            weightedTiles[tileType] = tileType.weight;
        }

        // Adjust weights based on neighbors
        foreach (var neighbor in GetNeighbors(x, y))
        {
            if (neighbor != null && neighbor.tileData != null)
            {
                TileSO neighborTileData = neighbor.tileData;

                foreach (var tileType in tileTypes)
                {
                    // Check if the current tile type prefers the neighbor
                    foreach (var preference in tileType.preferredNeighbors)
                    {
                        if (preference.neighborTile == neighborTileData)
                        {
                            // Increase weight for preferred neighbors
                            weightedTiles[tileType] += preference.weightBoost;
                        }
                    }
                }
            }
        }

        // Select a tile type based on the adjusted weights
        float totalWeight = 0f;
        foreach (var kvp in weightedTiles)
        {
            totalWeight += kvp.Value;
        }

        float randomValue = Random.Range(0, totalWeight);
        foreach (var kvp in weightedTiles)
        {
            if (randomValue < kvp.Value)
                return kvp.Key;
            randomValue -= kvp.Value;
        }

        // Fallback (shouldn't happen)
        return tileTypes[0];
    }


    Tile[] GetNeighbors(int x, int y)
    {
        List<Tile> neighbors = new List<Tile>();

        // Offset for flat-topped hex grid neighbors
        int[][] directions = new int[][]
        {
            new int[] { 1, 0 }, new int[] { -1, 0 }, // Horizontal
            new int[] { 0, 1 }, new int[] { 0, -1 }, // Vertical
            new int[] { 1, -1 }, new int[] { -1, 1 } // Diagonal
        };

        foreach (var dir in directions)
        {
            int nx = x + dir[0];
            int ny = y + dir[1];
            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                neighbors.Add(grid[nx, ny]);
            }
        }

        return neighbors.ToArray();
    }

    Vector2 CalculateHexPosition(int x, int y)
    {
        float xOffset = tileSize * 0.75f; // Horizontal spacing
        float yOffset = tileSize * Mathf.Sqrt(3) / 2f; // Vertical spacing

        float xPos = x * xOffset;
        float yPos = y * yOffset;

        // Offset every other column
        if (x % 2 == 1)
        {
            yPos += yOffset / 2f;
        }

        return new Vector2(xPos, yPos);
    }
}
