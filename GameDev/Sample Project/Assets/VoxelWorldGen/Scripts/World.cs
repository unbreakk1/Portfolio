using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;



public class World : MonoBehaviour
{
    [SerializeField] private int chunkSize = 16, chunkHeight = 100;
    [SerializeField] private int chunkDrawRange = 8;

    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private WorldRenderer worldRenderer;
    [SerializeField] private Vector2Int mapSeedOffset;

    public bool IsWorldCreated;
    public UnityEvent OnWorldCreated, OnNewChunksGenerated;

    private CancellationTokenSource taskTokenSource = new CancellationTokenSource();

    public WorldRenderer WorldRenderer => worldRenderer;
    public int ChunkSize => chunkSize;
    public int ChunkHeight => chunkHeight;
    public int ChunkDrawRange => chunkDrawRange;

    public WorldData WorldData { get; private set; }


    private void Awake()
    {
        WorldData = new WorldData
        {
            ChunkHeight = this.chunkHeight,
            ChunkSize = this.chunkSize,
            ChunkDataDictionary = new Dictionary<Vector3Int, ChunkData>(),
            ChunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>()
        };
    }
    
    public async void GenerateWorld()
    {
        await GenerateWorld(Vector3Int.zero);
    }

    private async Task GenerateWorld(Vector3Int position)
    {
        terrainGenerator.GenerateBiomePoints(position, chunkDrawRange, chunkSize, mapSeedOffset);

        WorldGenerationData worldGenerationData = await Task.Run(() => GetPositionInPlayerView(position));


        //Stays on MainThread -> not very expensive 
        foreach (Vector3Int pos in worldGenerationData.ChunkPositionsToRemove)
        {
            WorldDataHelper.RemoveChunk(this, pos);
        }

        foreach (Vector3Int pos in worldGenerationData.ChunkDataToRemove)
        {
            WorldDataHelper.RemoveChunkData(this, pos);
        }

        //thread safe dictionary to enable the use by multiple threads
        ConcurrentDictionary<Vector3Int, ChunkData> dataDictionary = null;

        try
        {
            dataDictionary = await CalculateWorldChunkData(worldGenerationData.ChunkDataPositionsToCreate);
        }
        catch (Exception)
        {
            
            return;
        }

        foreach (var calculatedData in dataDictionary)
        {
            WorldData.ChunkDataDictionary.Add(calculatedData.Key, calculatedData.Value);
        }

        foreach (var chunkData in WorldData.ChunkDataDictionary.Values)
        {
            AddTreeLeaves(chunkData);
        }

        ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary = new ConcurrentDictionary<Vector3Int, MeshData>();
        
        // gets ChunkData by looping through dictionaries
        List<ChunkData> dataToRender = WorldData.ChunkDataDictionary
            .Where(key => worldGenerationData.ChunkPositionToCreate.Contains(key.Key))
            .Select(keyValue => keyValue.Value).ToList();

        try
        {
            meshDataDictionary = await CreateMeshDataAsync(dataToRender);
        }
        catch (Exception)
        {
            
            return;
        }

        StartCoroutine(ChunkCreationCoroutine(meshDataDictionary));
    }

    private void AddTreeLeaves(ChunkData chunkData)
    {
        foreach (var leaf in chunkData.TreeData.TreeLeavesSolid)
        {
            Chunk.SetBlock(chunkData, leaf, BlockType.TreeLeafsSolid);
        }
    }

    private async Task<ConcurrentDictionary<Vector3Int, MeshData>> CreateMeshDataAsync(List<ChunkData> dataToRender)
    {
        ConcurrentDictionary<Vector3Int, MeshData> dictionary = new ConcurrentDictionary<Vector3Int, MeshData>();

        return await Task.Run(() =>
        {
            foreach (ChunkData data in dataToRender)
            {
                if (taskTokenSource.Token.IsCancellationRequested)
                {
                    taskTokenSource.Token.ThrowIfCancellationRequested();
                }

                MeshData meshData = Chunk.GetChunkMeshData(data);
                dictionary.TryAdd(data.WorldPosition, meshData);
            }

            return dictionary;
        },taskTokenSource.Token);
    }


    private IEnumerator ChunkCreationCoroutine(ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary)
    {
        foreach (var item in meshDataDictionary)
        {
            CreateChunk(WorldData, item.Key, item.Value);
            yield return new WaitForEndOfFrame();
        }

        if (IsWorldCreated == false)
        {
            IsWorldCreated = true;
            OnWorldCreated?.Invoke();
        }
    }

    private void CreateChunk(WorldData worldData, Vector3Int position, MeshData meshData)
    {
        ChunkRenderer chunkRenderer = worldRenderer.RenderChunk(worldData, position, meshData);
        worldData.ChunkDictionary.Add(position, chunkRenderer);
    }


    private async Task<ConcurrentDictionary<Vector3Int, ChunkData>> CalculateWorldChunkData(
        List<Vector3Int> chunkDataPositionsToCreate)
    {
        ConcurrentDictionary<Vector3Int, ChunkData> dictionary = new ConcurrentDictionary<Vector3Int, ChunkData>();

        return await Task.Run(() =>
        {
            foreach (Vector3Int pos in chunkDataPositionsToCreate)
            {
                if (taskTokenSource.Token.IsCancellationRequested)
                {
                    taskTokenSource.Token.ThrowIfCancellationRequested();
                }
                ChunkData data = new ChunkData(chunkSize, chunkHeight, this, pos);
                ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);
                dictionary.TryAdd(pos, newData);
            }

            return dictionary;
        },taskTokenSource.Token);
    }

    private WorldGenerationData GetPositionInPlayerView(Vector3Int playerPosition)
    {
        List<Vector3Int> allChunkPositionsNeeded = WorldDataHelper.GetChunkPositionsAroundPlayer(this, playerPosition);
        List<Vector3Int> allChunkDataPositionsNeeded =
            WorldDataHelper.GetDataPositionsAroundPlayer(this, playerPosition);

        List<Vector3Int> chunkPositionsToCreate =
            WorldDataHelper.SetPositionToCreate(WorldData, allChunkPositionsNeeded, playerPosition);

        List<Vector3Int> chunkDataPositionsToCreate =
            WorldDataHelper.SetDataPositionsToCreate(WorldData, allChunkDataPositionsNeeded, playerPosition);

        List<Vector3Int> chunkPositionsToRemove = WorldDataHelper.GetUnneededChunks(WorldData, allChunkPositionsNeeded);
        List<Vector3Int> chunkDataToRemove = WorldDataHelper.GetUnneededData(WorldData, allChunkDataPositionsNeeded);

        WorldGenerationData data = new WorldGenerationData
        {
            ChunkPositionToCreate = chunkPositionsToCreate,
            ChunkDataPositionsToCreate = chunkDataPositionsToCreate,
            ChunkPositionsToRemove = chunkPositionsToRemove,
            ChunkDataToRemove = chunkDataToRemove,
        };
        return data;
    }

    public BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.GetChunkPositionFromWorldCoords(this, x, y, z);
        ChunkData containerChunk = null;
        WorldData.ChunkDataDictionary.TryGetValue(pos, out containerChunk);

        if (containerChunk == null)
            return BlockType.Nothing;
        Vector3Int blockInChunkCoordinates = Chunk.GetBlockInChunkCoordinates(containerChunk, new Vector3Int(x, y, z));

        return Chunk.GetBlockFromChunkCoordinates(containerChunk, blockInChunkCoordinates);
    }

    public async void ChunkLoadRequest(GameObject player)
    {
        await GenerateWorld(Vector3Int.RoundToInt(player.transform.position));
        OnNewChunksGenerated?.Invoke();
    }

    private struct WorldGenerationData
    {
        public List<Vector3Int> ChunkPositionToCreate;
        public List<Vector3Int> ChunkDataPositionsToCreate;
        public List<Vector3Int> ChunkPositionsToRemove;

        public List<Vector3Int> ChunkDataToRemove;
    }

    public bool SetBlock(RaycastHit hit, BlockType blockType)
    {
        ChunkRenderer chunk = hit.collider.GetComponent<ChunkRenderer>();
        if (chunk == null)
            return false;

        Vector3Int pos = GetBlockPos(hit);

        WorldDataHelper.SetBlock(chunk.ChunkData.WorldReference, pos, blockType);
        chunk.ModifiedByPlayer = true;
        if (Chunk.IsOnEdge(chunk.ChunkData, pos))
        {
            List<ChunkData> neighbourDataList = Chunk.GetEdgeNeighbourChunk(chunk.ChunkData, pos);
            foreach (ChunkData neighbourData in neighbourDataList)
            {
                ChunkRenderer chunkToUpdate = WorldDataHelper.GetChunk(neighbourData.WorldReference,
                    neighbourData.WorldPosition);

                if (chunkToUpdate != null)
                    chunkToUpdate.UpdateChunk();
            }
        }

        chunk.UpdateChunk();
        return true;
    }

    private Vector3Int GetBlockPos(RaycastHit hit)
    {
        Vector3 pos = new Vector3(
            GetBlockPositionIn(hit.point.x, hit.normal.x),
            GetBlockPositionIn(hit.point.y, hit.normal.y),
            GetBlockPositionIn(hit.point.z, hit.normal.z)
        );
        return Vector3Int.RoundToInt(pos);
    }

    private float GetBlockPositionIn(float pos, float normal)
    {
        if (Mathf.Abs(pos % 1) == 0.5f)
            pos -= (normal / 2);

        return pos;
    }

    private void OnDisable()
    {
        taskTokenSource.Cancel();
    }
}

public struct WorldData
{
    public Dictionary<Vector3Int, ChunkData> ChunkDataDictionary;
    public Dictionary<Vector3Int, ChunkRenderer> ChunkDictionary;
    public int ChunkSize;
    public int ChunkHeight;
}