using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    [SerializeField] private NoiseSettings treeNoiseSettings;
    [SerializeField] private DomainWarping domainWarping;


    public TreeData GenerateTreeData(ChunkData chunkData, Vector2Int mapSeedOffset)
    {
        treeNoiseSettings.WorldOffset = mapSeedOffset;
        TreeData treeData = new();
        float[,] noiseData = GenerateTreeNoise(chunkData, treeNoiseSettings);
        treeData.TreePositions =
            DataProcessing.FindLocalMaxima(noiseData, chunkData.WorldPosition.x, chunkData.WorldPosition.z);

        return treeData;
    }

    private float[,] GenerateTreeNoise(ChunkData chunkData, NoiseSettings noiseSettings)
    {
        float[,] noiseMax = new float[chunkData.ChunkSize, chunkData.ChunkSize];
        int xMax = chunkData.WorldPosition.x + chunkData.ChunkSize;
        int xMin = chunkData.WorldPosition.x;
        int zMax = chunkData.WorldPosition.z + chunkData.ChunkSize;
        int zMin = chunkData.WorldPosition.z;
        int xIndex = 0, zIndex = 0;
        for (int x = xMin; x < xMax; x++)
        {
            for (int z = zMin; z < zMax; z++)
            {
                noiseMax[xIndex, zIndex] = domainWarping.GenerateDomainNoise(x, z, treeNoiseSettings);
                zIndex++;
            }

            xIndex++;
            zIndex = 0;
        }

        return noiseMax;
    }
}


