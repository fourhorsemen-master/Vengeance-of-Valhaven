using UnityEngine;

public static class MaterialExtensions
{
    private static readonly int emissiveColorId = Shader.PropertyToID("_EmissiveColor");
    private static readonly int emissiveMapId = Shader.PropertyToID("_EmissiveColorMap");
    private static readonly int unlitColorId = Shader.PropertyToID("_UnlitColor");
    private static readonly int colorId = Shader.PropertyToID("_BaseColor");

    public static Color GetEmissiveColour(this Material material)
    {
        return material.GetColor(emissiveColorId);
    }
    
    public static void SetEmissiveColour(this Material material, Color colour)
    {
        material.SetColor(emissiveColorId, colour);
    }

    public static Texture GetEmissiveMap(this Material material)
    {
        return material.GetTexture(emissiveMapId);
    }

    public static void SetEmissiveMap(this Material material, Texture map)
    {
        material.SetTexture(emissiveMapId, map);
    }

    public static void SetUnlitColour(this Material material, Color colour)
    {
        material.SetColor(unlitColorId, colour);
    }

    public static void SetColour(this Material material, Color colour)
    {
        material.SetColor(colorId, colour);
    }
}
