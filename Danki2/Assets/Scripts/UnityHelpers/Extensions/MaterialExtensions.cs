using UnityEngine;

namespace Assets.Scripts.UnityHelpers.Extensions
{
    public static class MaterialExtensions
    {
        public static void SetEmissiveColour(this Material material, Color colour)
        {
            material.SetColor("_EmissiveColor", colour);
        }

        public static void SetUnlitColour(this Material material, Color colour)
        {
            material.SetColor("_UnlitColor", colour);
        }

        public static void SetColour(this Material material, Color colour)
        {
            material.SetColor("Color", colour);
        }
    }
}
