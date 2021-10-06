using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SwordEmpowermentVisual : MonoBehaviour
{
    [SerializeField] private DecalProjector decal;

    [SerializeField] private TrailRenderer trail;


    public void Activate(Color colour, Material decalMaterial)
    {
        decal.fadeFactor = 1;
        decal.material = decalMaterial;

        trail.emitting = true;
        Color trailColour = Color.Lerp(colour, Color.white, 0.6f);
        trail.material.SetUnlitColour(trailColour);
    }

    public void Reset()
    {
        decal.fadeFactor = 0;
        trail.emitting = false;
    }
}
