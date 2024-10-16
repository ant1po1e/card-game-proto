using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class TileGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 20;
    public float caveFillPercent = 0.5f;

    public Tilemap tilemap;
    public RuleTile groundTile;

    void Start()
    {
    }

    public void GenerateTilemap()
    {
        ClearTilemap();

        // Initialize the map with random noise
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = Random.value < caveFillPercent ? 1 : 0;
            }
        }

        // Smooth the map
        for (int i = 0; i < 5; i++)
        {
            map = SmoothMap(map);
        }

        // Apply the map to the tilemap
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
                }
            }
        }
    }

    void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }

    int[,] SmoothMap(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallTiles = GetSurroundingWallCount(oldMap, x, y);

                if (oldMap[x, y] == 1)
                {
                    newMap[x, y] = (neighborWallTiles >= 4) ? 1 : 0;
                }
                else
                {
                    newMap[x, y] = (neighborWallTiles >= 5) ? 1 : 0;
                }
            }
        }

        return newMap;
    }

    int GetSurroundingWallCount(int[,] map, int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
            {
                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (neighborX != gridX || neighborY != gridY)
                    {
                        wallCount += map[neighborX, neighborY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
}