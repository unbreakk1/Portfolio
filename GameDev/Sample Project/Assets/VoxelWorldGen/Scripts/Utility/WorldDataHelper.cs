using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldDataHelper
{
    public static Vector3Int ChunkPositionFromBlockCoords(World world, Vector3Int position)
    {
        return new Vector3Int
        {
            x = Mathf.FloorToInt(position.x / (float)world.ChunkSize) * world.ChunkSize,
            y = Mathf.FloorToInt(position.y / (float)world.ChunkHeight) * world.ChunkHeight,
            z = Mathf.FloorToInt(position.z / (float)world.ChunkSize) * world.ChunkSize
        };
    }

    public static List<Vector3Int> GetChunkPositionsAroundPlayer(World world, Vector3Int playerPosition)
    {
        int startX = playerPosition.x - (world.ChunkDrawRange) * world.ChunkSize;
        int startZ = playerPosition.z - (world.ChunkDrawRange) * world.ChunkSize;
        int endX = playerPosition.x + (world.ChunkDrawRange) * world.ChunkSize;
        int endZ = playerPosition.z + (world.ChunkDrawRange) * world.ChunkSize;

        List<Vector3Int> chunkPositionsToCreate = new List<Vector3Int>();
        for (int x = startX; x <= endX; x += world.ChunkSize)
        {
            for (int z = startZ; z < endZ; z += world.ChunkSize)
            {
                Vector3Int chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, 0, z));
                chunkPositionsToCreate.Add(chunkPos);
                if (x >= playerPosition.x - world.ChunkSize &&
                    x <= playerPosition.x + world.ChunkSize &&
                    z >= playerPosition.z - world.ChunkSize &&
                    z <= playerPosition.z + world.ChunkSize)
                {
                    for (int y = -world.ChunkHeight;
                         y >= playerPosition.y - world.ChunkHeight * 2;
                         y -= world.ChunkHeight)
                    {
                        chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, y, z));
                        chunkPositionsToCreate.Add(chunkPos);
                    }
                }
            }
        }

        return chunkPositionsToCreate;
    }

    public static List<Vector3Int> GetDataPositionsAroundPlayer(World world, Vector3Int playerPosition)
    {
        int startX = playerPosition.x - (world.ChunkDrawRange ) * world.ChunkSize;
        int startZ = playerPosition.z - (world.ChunkDrawRange ) * world.ChunkSize;
        int endX = playerPosition.x + (world.ChunkDrawRange ) * world.ChunkSize;
        int endZ = playerPosition.z + (world.ChunkDrawRange ) * world.ChunkSize;

        List<Vector3Int> chunkDataPositionsToCreate = new List<Vector3Int>();
        for (int x = startX; x <= endX; x += world.ChunkSize)
        {
            for (int z = startZ; z < endZ; z += world.ChunkSize)
            {
                Vector3Int chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, 0, z));
                chunkDataPositionsToCreate.Add(chunkPos);
                if (x >= playerPosition.x - world.ChunkSize &&
                    x <= playerPosition.x + world.ChunkSize &&
                    z >= playerPosition.z - world.ChunkSize &&
                    z <= playerPosition.z + world.ChunkSize)
                {
                    for (int y = -world.ChunkHeight;
                         y >= playerPosition.y - world.ChunkHeight * 2;
                         y -= world.ChunkHeight)
                    {
                        chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, y, z));
                        chunkDataPositionsToCreate.Add(chunkPos);
                    }
                }
            }
        }

        return chunkDataPositionsToCreate;
    }

    public static List<Vector3Int> SetPositionToCreate(WorldData worldData,
        List<Vector3Int> allChunkPositionsNeeded, Vector3Int playerPosition)
    {
        return allChunkPositionsNeeded.Where(pos => worldData.ChunkDictionary.ContainsKey(pos) == false)
            .OrderBy(pos => Vector3.Distance(playerPosition, pos)).ToList();
    }

    public static List<Vector3Int> SetDataPositionsToCreate(WorldData worldData,
        List<Vector3Int> allChunkDataPositionsNeeded, Vector3Int playerPosition)
    {
        return allChunkDataPositionsNeeded.Where(pos => worldData.ChunkDataDictionary.ContainsKey(pos) == false)
            .OrderBy(pos => Vector3.Distance(playerPosition, pos)).ToList();
    }

    public static List<Vector3Int> GetUnneededChunks(WorldData worldData, List<Vector3Int> allChunkPositionsNeeded)
    {
        List<Vector3Int> positionsToRemove = new List<Vector3Int>();
        foreach (var pos in worldData.ChunkDataDictionary.Keys.Where(pos =>allChunkPositionsNeeded.Contains(pos) ==false))
        {
            if(worldData.ChunkDictionary.ContainsKey(pos))
                positionsToRemove.Add(pos);
        }

        return positionsToRemove;
    }

    public static List<Vector3Int> GetUnneededData(WorldData worldData, List<Vector3Int> allChunkDataPositionsNeeded)
    {
        return worldData.ChunkDataDictionary.Keys.Where(pos =>
            allChunkDataPositionsNeeded.Contains(pos) == false).ToList();

    }

    public static void RemoveChunk(World world, Vector3Int pos)
    {
        ChunkRenderer chunk = null;
        if (world.WorldData.ChunkDictionary.TryGetValue(pos, out chunk))
        {
            world.WorldRenderer.RemoveChunk(chunk);
            world.WorldData.ChunkDictionary.Remove(pos);
        }
        
    }

    public static void RemoveChunkData(World world, Vector3Int pos)
    {
        world.WorldData.ChunkDataDictionary.Remove(pos);
    }

    public static void SetBlock(World worldReference, Vector3Int pos, BlockType blockType)
    {
        ChunkData chunkData = GetChunkData(worldReference, pos);
        if (chunkData != null)
        {
            Vector3Int localPosition = Chunk.GetBlockInChunkCoordinates(chunkData, pos);
            Chunk.SetBlock(chunkData, localPosition, blockType);
        }
    }

    public static ChunkData GetChunkData(World worldReference, Vector3Int pos)
    {
        Vector3Int chunkPosition = ChunkPositionFromBlockCoords(worldReference, pos);

        ChunkData containerChunk = null;

        worldReference.WorldData.ChunkDataDictionary.TryGetValue(chunkPosition, out containerChunk);

        return containerChunk;
    }

    public static ChunkRenderer GetChunk(World worldReference, Vector3Int worldPosition)
    {
        if (worldReference.WorldData.ChunkDictionary.ContainsKey(worldPosition))
            return worldReference.WorldData.ChunkDictionary[worldPosition];
        return null;
    }
}