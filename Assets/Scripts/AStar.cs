using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<TileMono> GeneratePath(TileMono startTile, TileMono endTile)
    {
        List<TileMono> openSet = new List<TileMono>();

        foreach (TileMono tile in GridManager.Instance.grid)
        {
            tile.gScore = float.MaxValue;
        }

        startTile.gScore = 0;
        startTile.hScore = HexDistance(startTile, endTile);
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            int lowestF = 0;
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            TileMono currentTile = openSet[lowestF];
            openSet.Remove(currentTile);

            if (currentTile == endTile)

            {
                List<TileMono> path = new List<TileMono>();
                currentTile = endTile;

                while (currentTile != startTile)
                {
                    path.Add(currentTile);
                    currentTile = currentTile.cameFrom;
                }

                path.Reverse();
                return path;
            }

            foreach (TileMono neighbour in GridManager.Instance.GetNeighboursOfTIle(currentTile.tileX, currentTile.tileY))
            {
                float heldGScore = currentTile.gScore + HexDistance(currentTile, neighbour);

                if (heldGScore < neighbour.gScore)
                {
                    neighbour.cameFrom = currentTile;
                    neighbour.gScore = heldGScore;
                    neighbour.hScore = HexDistance(neighbour, endTile);

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    public float HexDistance(TileMono tileA, TileMono tileB)
    {
        //H = Max(|x1 – x2|, |y1 – y2|
        //USE ABS AS NEGATIVE DOES NOT WORK 
        int distanceX = Mathf.Abs(tileA.tileX - tileB.tileX);
        int distanceY = Mathf.Abs(tileA.tileY - tileB.tileY);
        return (distanceX + distanceY + Mathf.Abs(distanceX - distanceY)) / 2f;
    }

    public TileMono FindNearestTile(Vector2 pos)
    {
        TileMono foundTile = null;
        float minDistance = float.MaxValue;

        foreach (TileMono tile in GridManager.Instance.grid)
        {
            float currentDistance = Vector2.Distance(pos, tile.transform.position);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                foundTile = tile;
            }
        }

        return foundTile;
    }

    public TileMono FindFurthestTile(Vector2 pos)
    {
        TileMono foundTile = null;
        float maxDistance = default;

        foreach (TileMono tile in GridManager.Instance.grid)
        {
            float currentDistance = Vector2.Distance(pos, tile.transform.position);
            if (currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                foundTile = tile;
            }
        }

        return foundTile;
    }
}