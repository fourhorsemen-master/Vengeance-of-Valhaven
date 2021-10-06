using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class SwordEmpowermentVisual : MonoBehaviour
{
    [SerializeField] private DecalProjector decal;

    [SerializeField] private TrailRenderer trail;

    [SerializeField] private VisualEffect vfx;

    private const float TrailColourDamping = 0.6f;
    private const float VFXEmmissiveIntensity = 5f;

    public void Activate(Color colour, Material decalMaterial)
    {
        decal.fadeFactor = 1;
        decal.material = decalMaterial;

        trail.emitting = true;
        Color trailColour = Color.Lerp(colour, Color.white, TrailColourDamping);
        trail.material.SetUnlitColour(trailColour);

        vfx.SetVector4("Colour", colour * VFXEmmissiveIntensity);
        vfx.Play();
    }

    public void Reset()
    {
        decal.fadeFactor = 0;
        trail.emitting = false;
        vfx.Stop();
    }
}
