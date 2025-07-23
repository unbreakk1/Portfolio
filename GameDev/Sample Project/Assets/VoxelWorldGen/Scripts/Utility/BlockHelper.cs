using System;
using UnityEngine;

public static class BlockHelper
{
    private static Direction[] directions =
    {
        Direction.Backward,
        Direction.Down,
        Direction.Forward,
        Direction.Left,
        Direction.Right,
        Direction.Up
    };

    //meshData per block/face
    public static MeshData GetMeshData(ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        if (blockType == BlockType.Air || blockType == BlockType.Nothing)
            return meshData;
        
        foreach (Direction direction in directions)
        {
            var neighbourBlockCoords = new Vector3Int(x, y, z) + direction.GetVector();
            var neighbourBlockType = Chunk.GetBlockFromChunkCoordinates(chunk, neighbourBlockCoords);
            
            if (neighbourBlockType != BlockType.Nothing &&
                BlockDataManager.BlockTextureDictionary[neighbourBlockType].isSolid == false)
            {
                if (blockType == BlockType.Water)
                {
                    if (neighbourBlockType == BlockType.Air)
                        meshData.WaterMesh = GetFaceDataIn(direction, chunk, x, y, z, meshData.WaterMesh, blockType);
                }
                else
                {
                    meshData = GetFaceDataIn(direction, chunk, x, y, z, meshData, blockType);
                }
            }
        }

        return meshData;
    }
    
    private static MeshData GetFaceDataIn(Direction direction, ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        GetFaceVertices(direction,x,y,z,meshData,blockType);
        meshData.AddQuadTriangles(BlockDataManager.BlockTextureDictionary[blockType].generatesCollider);
        meshData.UV.AddRange(GetFaceUVs(direction,blockType));

        return meshData;
    }
    
    private static Vector2Int GetTexturePosition(Direction direction, BlockType blockType)
    {
        return direction switch
        {
            Direction.Up => BlockDataManager.BlockTextureDictionary[blockType].up,
            Direction.Down => BlockDataManager.BlockTextureDictionary[blockType].down,
            _ => BlockDataManager.BlockTextureDictionary[blockType].side
        };
    }

    //Generates UVs for the textures of each block side
    private static Vector2[] GetFaceUVs(Direction direction, BlockType blockType)
    {
        Vector2[] UVs = new Vector2[4];
        var tilePos = GetTexturePosition(direction, blockType);
        
        UVs[0] = new Vector2(
            BlockDataManager.TileSizeX * tilePos.x + BlockDataManager.TileSizeX - BlockDataManager.TextureOffset,
            BlockDataManager.TileSizeY * tilePos.y + BlockDataManager.TextureOffset);

        UVs[1] = new Vector2(
            BlockDataManager.TileSizeX * tilePos.x + BlockDataManager.TileSizeX - BlockDataManager.TextureOffset,
            BlockDataManager.TileSizeY * tilePos.y + BlockDataManager.TileSizeY - BlockDataManager.TextureOffset);

        UVs[2] = new Vector2(
            BlockDataManager.TileSizeX * tilePos.x + BlockDataManager.TextureOffset,
            BlockDataManager.TileSizeY * tilePos.y + BlockDataManager.TileSizeY - BlockDataManager.TextureOffset);

        UVs[3] = new Vector2(
            BlockDataManager.TileSizeX * tilePos.x + BlockDataManager.TextureOffset,
            BlockDataManager.TileSizeY * tilePos.y + BlockDataManager.TextureOffset);

        return UVs;
    }

    
    //Generates vertices for each face of the Block/Voxel, x,y,z are Center points, to calculate the vertices we need
    //to + or - 0.5
    private static void GetFaceVertices(Direction direction, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        var generatesCollider = BlockDataManager.BlockTextureDictionary[blockType].generatesCollider;

        switch (direction)
        {
            case Direction.Backward:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;
            case Direction.Forward:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.Left:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;
            case Direction.Right:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.Down:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.Up:
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}