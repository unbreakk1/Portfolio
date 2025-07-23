using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private BiomeGenerator biomeGenerator;
    [SerializeField, Expandable] private NoiseSettings biomeNoiseSettings;
    [SerializeField] private DomainWarping domainWarping;
    [SerializeField] private List<Vector3Int> biomeCenters = new();
    [SerializeField] private List<BiomesData> biomeGeneratorData = new();
    [SerializeField] private List<float> biomeNoise = new();

    public ChunkData GenerateChunkData(ChunkData data, Vector2Int mapSeedOffset)
    {
        BiomeGeneratorSelection biomeSelection = SelectBiomeGenerator(data.WorldPosition,data, false);
        
        data.TreeData = biomeSelection.biomeGenerator.GetTreeData(data, mapSeedOffset);
        for (int x = 0; x < data.ChunkSize; x++)
        {
            for (int z = 0; z < data.ChunkSize; z++)
            {
                biomeSelection = SelectBiomeGenerator(new Vector3Int(data.WorldPosition.x+x, 0,data.WorldPosition.z+z),data);
                data = biomeSelection.biomeGenerator.ProcessChunkColumn(data, x, z, mapSeedOffset,biomeSelection.terrainSurfaceNoise);
            }
        }

        return data;
    }

    private BiomeGeneratorSelection SelectBiomeGenerator(Vector3Int worldPosition,ChunkData data, bool useDomainWarping = true)
    {
        if (useDomainWarping == true)
        {
            Vector2Int domainOffset = Vector2Int.RoundToInt(domainWarping.GenerateDomainOffset(worldPosition.x, worldPosition.z));
            worldPosition += new Vector3Int(domainOffset.x, domainOffset.y);
        }

        List<BiomeSelectionHelper> biomeSelectionHelpers = GetBiomeSelectionHelpers(worldPosition);

        BiomeGenerator generator1 = SelectBiome(biomeSelectionHelpers[0].Index);
        BiomeGenerator generator2 = SelectBiome(biomeSelectionHelpers[1].Index);

        float distance = Vector3.Distance(biomeCenters[biomeSelectionHelpers[0].Index],
                                          biomeCenters[biomeSelectionHelpers[1].Index]);
        float weight0 = biomeSelectionHelpers[1].Distance / distance;
        float weight1 = 1 - weight0;

        int terrainHeightNoise0 = generator1.GetSurfaceHeightNoise(worldPosition.x, worldPosition.z, data.ChunkHeight);
        int terrainHeightNoise1 = generator2.GetSurfaceHeightNoise(worldPosition.x, worldPosition.z, data.ChunkHeight);
        
        return new BiomeGeneratorSelection(generator1,
            Mathf.RoundToInt(terrainHeightNoise0 * weight0 + terrainHeightNoise1 * weight1));
    }

    private BiomeGenerator SelectBiome(int index)
    {
        float temp = biomeNoise[index];
        foreach (var data in biomeGeneratorData)
        {
            if (temp >= data.TemperatureStartThreshold && temp < data.TemperaturEndThreshold)
                return data.biomeTerrainGenerator;
        }

        return biomeGeneratorData[0].biomeTerrainGenerator;
    }

    private List<BiomeSelectionHelper> GetBiomeSelectionHelpers(Vector3Int position)
    {
        position.y = 0;
        return GetClosestBiomeIndex(position);
    }

    private List<BiomeSelectionHelper> GetClosestBiomeIndex(Vector3Int position)
    {
        return biomeCenters.Select((center, index) =>
            new BiomeSelectionHelper
            {
                Index = index,
                Distance = Vector3.Distance(center,position)
            }).OrderBy(helper => helper.Distance).Take(8).ToList();
    }

    private struct BiomeSelectionHelper
    {
        public int Index;
        public float Distance;
    }

    public void GenerateBiomePoints(Vector3 playerPosition, int drawRange, int mapSize, Vector2Int mapSeedOffset)
    {
        biomeCenters = new List<Vector3Int>();
        biomeCenters = BiomeCenterFinder.CalculateBiomeCenters(playerPosition, drawRange, mapSize);

        for (int i = 0; i < biomeCenters.Count; i++)
        {
            Vector2Int domainWarpintOffset =
                domainWarping.GenerateDomainOffsetInt(biomeCenters[i].x, biomeCenters[i].y);
            biomeCenters[i] += new Vector3Int(domainWarpintOffset.x, 0, domainWarpintOffset.y);
        }

        biomeNoise = CalculateBiomeNoise(biomeCenters, mapSeedOffset);
    }

    private List<float> CalculateBiomeNoise(List<Vector3Int> biomeCenters, Vector2Int mapSeedOffset)
    {
        biomeNoiseSettings.WorldOffset = mapSeedOffset;
        return biomeCenters.Select(center => MyNoise.GetOctavePerlin(center.x, center.y, biomeNoiseSettings)).ToList();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (var centerPoint in biomeCenters)
        {
            Gizmos.DrawLine(centerPoint, centerPoint + Vector3.up * 255);
        }
    }
}

[Serializable]
public struct BiomesData
{
    [Range(0, 1)] public float TemperatureStartThreshold, TemperaturEndThreshold;
    public BiomeGenerator biomeTerrainGenerator;
}