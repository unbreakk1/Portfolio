using UnityEngine;

public static class MyNoise
{ 
    public static float RemapValues(float value, float initialMin, float initialMax, float outputMin, float outputMax)
    {
        return outputMax + (value - initialMin) * (outputMax - initialMax) / (initialMax - initialMin);
    }

    private static float RemapValues01(float value, float outputMin, float outputMax)
    {
        return outputMin + (value - 0) * (outputMax - outputMin) / (1 - 0);
    }

    public static int RemapValues01ToInt(float value, float outputMin, float outputMax)
    {
        return (int)RemapValues01(value, outputMin, outputMax);
    }

    public static float DoRedistribution(float noise, NoiseSettings settings)
    {
        return Mathf.Pow(noise * settings.RedistributionModifier, settings.Exponent);
    }
    
    public static float GetOctavePerlin(float x, float z, NoiseSettings settings)
    {
        x *= settings.NoiseZoom;
        z *= settings.NoiseZoom;
        x += settings.NoiseZoom;
        z += settings.NoiseZoom;
        float total = 0;
        float frequency = settings.frequency;
        float amplitude = settings.amplitude;
        float amplitudeSum = 1; // normalizes result to 0.0 - 1.0

        for (int i = 0; i < settings.Octaves; i++)
        {
            total += Mathf.PerlinNoise((settings.Offset.x + settings.WorldOffset.x + x) * frequency,
                (settings.Offset.y + settings.WorldOffset.y + z) * frequency) * amplitude;

            amplitudeSum += amplitude;

            amplitude *= settings.Persistance;
            frequency *= 2;
        }

        return total / amplitudeSum;
    }
}