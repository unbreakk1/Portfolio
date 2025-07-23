using UnityEngine;

public class ChunkData
{
    private int chunkSize = 16;
    private int chunkHeight = 100;
    public bool ModifiedByPlayer = false;

    private BlockType[] blocks;

    private World worldReference;
    private Vector3Int worldPosition;
    public TreeData TreeData;


    public int ChunkSize  => chunkSize;
    public int ChunkHeight => chunkHeight;
    public BlockType[] Blocks => blocks;
    public Vector3Int WorldPosition { get => worldPosition; set => worldPosition = value; }
    public World WorldReference { get => worldReference; set => worldReference = value; }

    public ChunkData(int chunkSize, int chunkHeight, World world, Vector3Int worldPosition)
    {
        this.chunkHeight = chunkHeight;
        this.chunkSize = chunkSize;
        this.worldReference = world;
        this.worldPosition = worldPosition;
        blocks = new BlockType[chunkSize * chunkHeight * chunkSize];
    }
}