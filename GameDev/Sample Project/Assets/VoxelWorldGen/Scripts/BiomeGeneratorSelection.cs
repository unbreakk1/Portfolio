public class BiomeGeneratorSelection
{
    public BiomeGenerator biomeGenerator = null;
    public int? terrainSurfaceNoise = null;

    public BiomeGeneratorSelection(BiomeGenerator biomeGenerator, int? terrainSurfaceNoise = null)
    {
        this.biomeGenerator = biomeGenerator;
        this.terrainSurfaceNoise = terrainSurfaceNoise;
    }
}
