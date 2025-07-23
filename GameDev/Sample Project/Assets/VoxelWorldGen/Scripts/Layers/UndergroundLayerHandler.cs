using UnityEngine;

public class UndergroundLayerHandler : BlockLayerHandler
{
    [SerializeField] private BlockType undergroundBlockType;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise,
        Vector2Int mapSeedOffset)
    {
        if (y < surfaceHeightNoise)
        {
            Vector3Int pos = new Vector3Int(x, y - chunkData.WorldPosition.y, z);
            Chunk.SetBlock(chunkData, pos, undergroundBlockType);
            return true;
        }

        return false;
    }
}