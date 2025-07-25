using System.Collections.Generic;
using UnityEngine;

public class TreeLayerHandler : BlockLayerHandler
{
    [SerializeField] private float terrainHeightLimit = 25;

    public static List<Vector3Int> TreeLeavesStaticLayout = new List<Vector3Int>
    {
        new Vector3Int(-2, 0, -2),
        new Vector3Int(-2, 0, -1),
        new Vector3Int(-2, 0, 0),
        new Vector3Int(-2, 0, 1),
        new Vector3Int(-2, 0, 2),
        new Vector3Int(-1, 0, -2),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(-1, 0, 2),
        new Vector3Int(0,0,-2),
        new Vector3Int(0,0,-1),
        new Vector3Int(0,0,0),
        new Vector3Int(0,0,1),
        new Vector3Int(0,0,2),
        new Vector3Int(1,0,-2),
        new Vector3Int(1,0,-1),
        new Vector3Int(1,0,0),
        new Vector3Int(1,0,1),
        new Vector3Int(1,0,2),
        new Vector3Int(2,0,-2),
        new Vector3Int(2,0,-1),
        new Vector3Int(2,0,0),
        new Vector3Int(2,0,1),
        new Vector3Int(2,0,2),
        new Vector3Int(-1,1,-1),
        new Vector3Int(-1,1,0),
        new Vector3Int(-1,1,1),
        new Vector3Int(0,1,-1),
        new Vector3Int(0,1,0),
        new Vector3Int(0,1,1),
        new Vector3Int(1,1,-1),
        new Vector3Int(1,1,0),
        new Vector3Int(1,1,1),
        new Vector3Int(0,2,0),
    };


    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise,
        Vector2Int mapSeedOffset)
    {
        if (chunkData.WorldPosition.y < 0)
            return false;

        if (surfaceHeightNoise < terrainHeightLimit
            && chunkData.TreeData.TreePositions.Contains(new Vector2Int(chunkData.WorldPosition.x + x,
                chunkData.WorldPosition.z + z)))
        {
            Vector3Int chunkCoordinates = new Vector3Int(x, surfaceHeightNoise, z);

            BlockType type = Chunk.GetBlockFromChunkCoordinates(chunkData, chunkCoordinates);

            if (type == BlockType.GrassDirt)
            {
                Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.Dirt);

                for (int i = 0; i < 5; i++)
                {
                    chunkCoordinates.y = surfaceHeightNoise + i;
                    Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.TreeTrunk);
                }

                foreach (Vector3Int leafPosition in TreeLeavesStaticLayout)
                {
                    chunkData.TreeData.TreeLeavesSolid.Add(new Vector3Int(x + leafPosition.x,
                        surfaceHeightNoise + 5 + leafPosition.y, z + leafPosition.z));
                }
            }
        }

        return false;
    }
}