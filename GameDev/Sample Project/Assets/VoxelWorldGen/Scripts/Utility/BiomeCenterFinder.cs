using System.Collections.Generic;
using UnityEngine;

public static class BiomeCenterFinder
{
    private static List<Vector2Int> neighbourDirections = new()
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1)
    };

    
    //calculates the BiomeCenters and returns a noise value(temp)
    public static List<Vector3Int> CalculateBiomeCenters(Vector3 playerPosition, int drawRange, int mapSize)
    {
        int biomeLength = drawRange * mapSize;

        Vector3Int origin = new Vector3Int(Mathf.RoundToInt(playerPosition.x / biomeLength) * biomeLength,
            0, Mathf.RoundToInt(playerPosition.z / biomeLength) * biomeLength);

        HashSet<Vector3Int> biomeCenterTemp = new HashSet<Vector3Int>();

        biomeCenterTemp.Add(origin);

        foreach (Vector2Int offSet in neighbourDirections)
        {
            Vector3Int newBiomePoint1 = new Vector3Int(origin.x + offSet.x * biomeLength,
                0,
                origin.z + offSet.y * biomeLength);
            Vector3Int newBiomePoint2 = new Vector3Int(origin.x + offSet.x * biomeLength,
                0,
                origin.z * offSet.y * 2 * biomeLength);
            Vector3Int newBiomePoint3 = new Vector3Int(origin.x + offSet.x * 2 * biomeLength, 0,
                origin.z + offSet.y * biomeLength);
            Vector3Int newBiomePoint4 = new Vector3Int(origin.x + offSet.x * 2 * biomeLength, 0,
                origin.z + offSet.y * 2 * biomeLength);
            biomeCenterTemp.Add(newBiomePoint1);
            biomeCenterTemp.Add(newBiomePoint2);
            biomeCenterTemp.Add(newBiomePoint3);
            biomeCenterTemp.Add(newBiomePoint4);
        }

        return new List<Vector3Int>(biomeCenterTemp);
    }
}