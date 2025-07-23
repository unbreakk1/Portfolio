using UnityEngine;

[CreateAssetMenu(fileName = "NoiseSettings",menuName = "Data/Noise Settings")]
public class NoiseSettings: ScriptableObject
{
    public string name;
    
    public float RedistributionModifier;
    public float NoiseZoom;
    public int Octaves;
    public float Persistance;
    public float frequency;
    public float amplitude;

    public Vector2Int Offset;
    public Vector2Int WorldOffset;
    public float Exponent;
}
