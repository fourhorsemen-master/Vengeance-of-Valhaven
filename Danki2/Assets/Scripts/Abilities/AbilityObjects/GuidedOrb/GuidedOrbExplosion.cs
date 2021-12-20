using UnityEngine;
using UnityEngine.VFX;

public class GuidedOrbExplosion : AbilityObject
{
    [SerializeField]
    private VisualEffect visualEffect;

    public static void Create(GuidedOrbExplosion prefab, Vector3 position)
    {
        VisualEffect vfx = Instantiate(prefab, position, Quaternion.identity).visualEffect;

        vfx.SetVector4("Colour", VisualSettings.Instance.EnergyColour);
    }
}
