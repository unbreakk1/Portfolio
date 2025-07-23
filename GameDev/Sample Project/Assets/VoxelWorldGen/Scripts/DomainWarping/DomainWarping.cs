using UnityEngine;

public class DomainWarping : MonoBehaviour
{
    [SerializeField] private NoiseSettings noiseDomainX, noiseDomainY;
    [SerializeField] private int amplitudeX = 20, amplitudeY = 20;


    public float GenerateDomainNoise(int x, int z, NoiseSettings defaultNoiseSettings)
    {
        Vector2 domainOffset = GenerateDomainOffset(x, z);
        return MyNoise.GetOctavePerlin(x + domainOffset.x, z + domainOffset.y, defaultNoiseSettings);
    }

    public Vector2 GenerateDomainOffset(int x, int z)
    {
        var noiseX = MyNoise.GetOctavePerlin(x, z, noiseDomainX) * amplitudeX;
        var noiseY = MyNoise.GetOctavePerlin(x, z, noiseDomainY) * amplitudeY;

        return new Vector2(noiseX, noiseY);
    }

    public Vector2Int GenerateDomainOffsetInt(int x, int z)
    {
        return Vector2Int.RoundToInt(GenerateDomainOffset(x, z));
    }
}
