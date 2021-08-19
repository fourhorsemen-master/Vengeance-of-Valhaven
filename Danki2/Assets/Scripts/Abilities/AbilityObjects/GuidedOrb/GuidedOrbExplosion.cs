using UnityEngine;
using UnityEngine.VFX;

public class GuidedOrbExplosion : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private VisualEffect visualEffect;

    public static void Create(GuidedOrbExplosion prefab, Vector3 position)
    {
        VisualEffect vfx = Instantiate(prefab, position, Quaternion.identity).visualEffect;

        vfx.SetVector4("Colour", VisualSettings.Instance.EnergyColour);
    }
}
