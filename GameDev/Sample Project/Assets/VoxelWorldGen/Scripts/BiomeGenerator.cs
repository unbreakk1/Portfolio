using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    [SerializeField] private bool useDomainWarping = true;
    [SerializeField, Expandable] private NoiseSettings biomeNoiseSettings;
    [SerializeField] private DomainWarping domainWarping;
    [SerializeField] private TreeGenerator treeGenerator;
    [SerializeField] private BlockLayerHandler startLayerHandler;
    [SerializeField] private List<BlockLayerHandler> additionalHandlers = new();

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int mapSeedOffset,int? terrainHeightNoise)
    {
        biomeNoiseSettings.WorldOffset = mapSeedOffset;
        int groundPosition;
        if (terrainHeightNoise.HasValue == false)
            groundPosition =
                GetSurfaceHeightNoise(data.WorldPosition.x + x, data.WorldPosition.z + z, data.ChunkHeight);
        else
            groundPosition = terrainHeightNoise.Value;
                
            

        for (int y = data.WorldPosition.y; y < data.WorldPosition.y + data.ChunkHeight; y++)
        {
            startLayerHandler.Handle(data, x, y, z, groundPosition, mapSeedOffset);
        }

        foreach (var layers in additionalHandlers)
        {
            layers.Handle(data, x, data.WorldPosition.y, z, groundPosition, mapSeedOffset);
        }

        return data;
    }

    public int GetSurfaceHeightNoise(int x, int z, int chunkHeight)
    {
        float terrainHeight;
        if (useDomainWarping == false)
            terrainHeight = MyNoise.GetOctavePerlin(x, z, biomeNoiseSettings);
        else
            terrainHeight = domainWarping.GenerateDomainNoise(x, z, biomeNoiseSettings);


        terrainHeight = MyNoise.DoRedistribution(terrainHeight, biomeNoiseSettings);
        int surfaceHeight = MyNoise.RemapValues01ToInt(terrainHeight, 0, chunkHeight);

        return surfaceHeight;
    }

    public TreeData GetTreeData(ChunkData data, Vector2Int mapSeedOffset)
    {
        if (treeGenerator == null)
            return new TreeData();

        return treeGenerator.GenerateTreeData(data, mapSeedOffset);
    }
}