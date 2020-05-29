using UnityEngine;

namespace Assets.Scripts.UnityHelpers.Extensions
{
    public static class MaterialExtensions
    {
        public static void SetEmissiveColour(this Material material, Color colour)
        {
            material.SetColor("_EmissiveColor", colour);
        }
    }
}
