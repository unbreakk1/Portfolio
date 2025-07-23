using System.Collections.Generic;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public static float TextureOffset = 0.01f;
    public static float TileSizeX, TileSizeY;
    public static Dictionary<BlockType, TextureData> BlockTextureDictionary = new();
    public BlockDataSO textureData;


    private void Awake()
    {
        foreach (var item in textureData.textureDataList)
        {
            BlockTextureDictionary.Add(item.blockType, item);
        }

        TileSizeX = textureData.textureSizeX;
        TileSizeY = textureData.textureSizeY;
    }
}
