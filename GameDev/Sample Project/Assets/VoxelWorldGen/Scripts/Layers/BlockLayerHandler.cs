using UnityEngine;

public abstract class BlockLayerHandler : MonoBehaviour
{
    [SerializeField] private BlockLayerHandler next;

    public bool Handle(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (TryHandling(chunkData, x, y, z, surfaceHeightNoise, mapSeedOffset))
            return true;
        if (next != null)
            return next.Handle(chunkData, x, y, z, surfaceHeightNoise, mapSeedOffset);

        return false;
    }

    protected abstract bool TryHandling(ChunkData chunkData, int i, int i1, int i2, int surfaceHeightNoise,
        Vector2Int mapSeedOffset);



}
