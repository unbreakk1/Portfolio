using UnityEngine;

public class StoneLayerHandler : BlockLayerHandler
{
    [SerializeField, Range(0, 1)] private float stoneThreshold;

    [SerializeField] private NoiseSettings stoneNoiseSettings;
    [SerializeField] private DomainWarping domainWarping;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise,
        Vector2Int mapSeedOffset)
    {
        if (chunkData.WorldPosition.y > surfaceHeightNoise)
            return false;

        stoneNoiseSettings.WorldOffset = mapSeedOffset;
        float stoneNoise = MyNoise.GetOctavePerlin(chunkData.WorldPosition.x + x, chunkData.WorldPosition.z + z,
                            stoneNoiseSettings);
       // float stoneNoise = domainWarping.GenerateDomainNoise(chunkData.WorldPosition.x + x,
       //     chunkData.WorldPosition.z + z,
       //     stoneNoiseSettings);

        int endPosition = surfaceHeightNoise;
        if (chunkData.WorldPosition.y < 0)
            endPosition = chunkData.WorldPosition.y + chunkData.ChunkHeight;

        if (stoneNoise > stoneThreshold)
        {
            for (int i = chunkData.WorldPosition.y; i <= endPosition; i++)
            {
                Vector3Int pos = new Vector3Int(x, i, z);
                Chunk.SetBlock(chunkData, pos, BlockType.Stone);
            }

            return true;
        }

        return false;
    }
}