using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    [SerializeField] private GameObject chunkPrefab;
    private Queue<ChunkRenderer> chunkPool = new Queue<ChunkRenderer>();


    public void ClearPool(WorldData worldData)
    {
        foreach (var item in worldData.ChunkDictionary.Values)
        {
            Destroy(item.GameObject());
        }
        chunkPool.Clear();
    }
    
    public ChunkRenderer RenderChunk(WorldData worldData, Vector3Int position, MeshData meshData)
    {
        ChunkRenderer newChunk = null;
        if (chunkPool.Count > 0)
        {
            newChunk = chunkPool.Dequeue();
            newChunk.transform.position = position;
        }
        else
        {
            GameObject chunkObject = Instantiate(chunkPrefab, position, Quaternion.identity, transform);
            newChunk = chunkObject.GetComponent<ChunkRenderer>();
        }
        
        newChunk.InitializeChunk(worldData.ChunkDataDictionary[position]);
        newChunk.UpdateChunk(meshData);
        newChunk.gameObject.SetActive(true);

        return newChunk;
    }

    public void RemoveChunk(ChunkRenderer chunk)
    {
        chunk.gameObject.SetActive(false);
        chunkPool.Enqueue(chunk);
    }
}
