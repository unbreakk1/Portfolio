using System;
using System.Collections.Generic;
using UnityEngine;

public static class Chunk
{
    //All block meshData is calculated
    public static MeshData GetChunkMeshData(ChunkData chunkData)
    {
        MeshData meshData = new MeshData(true);
        
        LoopBlocks(chunkData,
            (x, y, z) => meshData = BlockHelper.GetMeshData(chunkData, x, y, z, meshData,
                chunkData.Blocks[GetIndexFromPosition(chunkData, x, y, z)]));

        return meshData;
    }

    private static void LoopBlocks(ChunkData chunkData, Action<int, int, int> actionToPerform)
    {
        for (int i = 0; i < chunkData.Blocks.Length; i++)
        {
            var position = GetPositionFromIndex(chunkData, i);
            actionToPerform(position.x, position.y, position.z);
        }
    }

    private static Vector3Int GetPositionFromIndex(ChunkData chunkData, int index)
    {
        
        //https://stackoverflow.com/questions/11316490/convert-a-1d-array-index-to-a-3d-array-index 8)
        int x = index % chunkData.ChunkSize;
        int y = (index / chunkData.ChunkSize) % chunkData.ChunkHeight;
        int z = index / (chunkData.ChunkSize * chunkData.ChunkHeight);
        return new Vector3Int(x, y, z);
    }

    private static bool InRange(ChunkData chunkData, int axisCoord)
    {
        if (axisCoord < 0 || axisCoord >= chunkData.ChunkSize)
            return false;

        return true;
    }

    //in chunk Coord system ( 0 - maxheight (100))
    private static bool InRangeHeight(ChunkData chunkData, int yCoord)
    {
        if (yCoord < 0 || yCoord >= chunkData.ChunkHeight)
            return false;

        return true;
    }

    public static void SetBlock(ChunkData chunkData, Vector3Int localPosition, BlockType block)
    {
            
        if (InRange(chunkData, localPosition.x) && InRangeHeight(chunkData, localPosition.y) &&
            InRange(chunkData, localPosition.z))
        {
            int index = GetIndexFromPosition(chunkData, localPosition.x, localPosition.y, localPosition.z);
            
            chunkData.Blocks[index] = block;
        }
        else
            WorldDataHelper.SetBlock(chunkData.WorldReference, localPosition + chunkData.WorldPosition, block);
    }

    private static int GetIndexFromPosition(ChunkData chunkData, int x, int y, int z)
    {
        //https://stackoverflow.com/questions/11316490/convert-a-1d-array-index-to-a-3d-array-index 8)
        return x + chunkData.ChunkSize * y + chunkData.ChunkSize * chunkData.ChunkHeight * z;
    }

    // Vector3 worldPosition to chunk Coordinates
    public static Vector3Int GetBlockInChunkCoordinates(ChunkData chunkData, Vector3Int position)
    {
        return new Vector3Int
        {
            x = position.x - chunkData.WorldPosition.x,
            y = position.y - chunkData.WorldPosition.y,
            z = position.z - chunkData.WorldPosition.z
        };
    }

    private static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        if (InRange(chunkData, x) && InRangeHeight(chunkData, y) && InRange(chunkData, z))
        {
            int index = GetIndexFromPosition(chunkData, x, y, z);
            return chunkData.Blocks[index];
        }

        return chunkData.WorldReference.GetBlockFromChunkCoordinates(chunkData, chunkData.WorldPosition.x + x,
            chunkData.WorldPosition.y + y, chunkData.WorldPosition.z + z);
    }

    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int chunkCoord)
    {
        return GetBlockFromChunkCoordinates(chunkData, chunkCoord.x, chunkCoord.y, chunkCoord.z);
    }

    public static Vector3Int GetChunkPositionFromWorldCoords(World world, int x, int y, int z)
    {
        Vector3Int pos = new Vector3Int
        {
            x = Mathf.FloorToInt(x / (float)world.ChunkSize) * world.ChunkSize,
            y = Mathf.FloorToInt(y / (float)world.ChunkHeight) * world.ChunkHeight,
            z = Mathf.FloorToInt(z / (float)world.ChunkSize) * world.ChunkSize
        };
        return pos;
    }

    public static bool IsOnEdge(ChunkData chunkData, Vector3Int worldPosition)
    {
        Vector3Int chunkPosition = GetBlockInChunkCoordinates(chunkData, worldPosition);
        if (chunkPosition.x == 0 || chunkPosition.x == chunkData.ChunkSize - 1 ||
            chunkPosition.y == 0 || chunkPosition.y == chunkData.ChunkHeight - 1 ||
            chunkPosition.z == 0 || chunkPosition.z == chunkData.ChunkSize - 1)
            return true;

        return false;
    }

    
    //fixes chunk edges if one block is removed, there will be no hole
    public static List<ChunkData> GetEdgeNeighbourChunk(ChunkData chunkData, Vector3Int worldPosition)
    {
        Vector3Int chunkPosition = GetBlockInChunkCoordinates(chunkData, worldPosition);
        List<ChunkData> neighboursToUpdate = new List<ChunkData>();
        if(chunkPosition.x == 0)
            neighboursToUpdate.Add(WorldDataHelper.GetChunkData(chunkData.WorldReference, worldPosition - Vector3Int.right));
        if(chunkPosition.x == chunkData.ChunkSize -1) 
            neighboursToUpdate.Add(WorldDataHelper.GetChunkData(chunkData.WorldReference, worldPosition + Vector3Int.right));
        if(chunkPosition.y == 0)
            neighboursToUpdate.Add(WorldDataHelper.GetChunkData(chunkData.WorldReference, worldPosition - Vector3Int.up));
        if(chunkPosition.y == chunkData.ChunkHeight -1)
            neighboursToUpdate.Add(WorldDataHelper.GetChunkData(chunkData.WorldReference, worldPosition + Vector3Int.up)); 
        if(chunkPosition.z == 0)
            neighboursToUpdate.Add(WorldDataHelper.GetChunkData(chunkData.WorldReference, worldPosition - Vector3Int.forward));
        if(chunkPosition.z == chunkData.ChunkSize -1)
            neighboursToUpdate.Add(WorldDataHelper.GetChunkData(chunkData.WorldReference, worldPosition + Vector3Int.forward));

        return neighboursToUpdate;
    }
}