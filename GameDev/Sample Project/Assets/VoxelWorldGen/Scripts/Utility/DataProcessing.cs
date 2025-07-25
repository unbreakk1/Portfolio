﻿using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataProcessing
{

    public static List<Vector2Int> Directions = new List<Vector2Int>
    {
        new Vector2Int(0,1),    //N
        new Vector2Int(1,1),    //NE
        new Vector2Int(1,0),    //E
        new Vector2Int(-1,1),   //SE
        new Vector2Int(-1,0),   //S
        new Vector2Int(-1,-1),  //SW
        new Vector2Int(0,-1),   //W
        new Vector2Int(1,-1)    //NW
    };
    
    public static List<Vector2Int> FindLocalMaxima(float[,] dataMatrix, int xCoord, int zCoord)
    {
        List<Vector2Int> maximas = new();
        for (int x = 0; x < dataMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < dataMatrix.GetLength(1); y++)
            {
                float noiseVal = dataMatrix[x, y];
                if (CheckNeighbours(dataMatrix, x, y, (neighbourNoise) => neighbourNoise < noiseVal))
                    maximas.Add(new Vector2Int(xCoord + x, zCoord + y));
            }
        }

        return maximas;
    }

    private static bool CheckNeighbours(float[,] dataMatrix, int x, int y, Func<float, bool> successCondition)
    {
        foreach (var dir in Directions)
        {
            var newPos = new Vector2Int(x + dir.x, y + dir.y);

            if (newPos.x < 0 || newPos.x >= dataMatrix.GetLength(0) || newPos.y < 0 ||
                newPos.y >= dataMatrix.GetLength(1))
                continue;

            if (successCondition(dataMatrix[x + dir.x, y + dir.y]) == false)
                return false;
        }

        return true;

    }
    
}
