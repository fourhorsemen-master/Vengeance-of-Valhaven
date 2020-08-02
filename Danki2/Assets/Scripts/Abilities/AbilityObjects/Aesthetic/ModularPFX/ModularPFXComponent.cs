using UnityEngine;

public class ModularPFXComponent : MonoBehaviour
{
    const string ColourKeyString = "Color_33632292";
    const string EmissiveKeyString = "Color_A9688267";

    [SerializeField, HideInInspector]
    Material mpfxMaterial = null;

    [SerializeField]
    private MPFXSettings settings;

    [SerializeField]
    private MPFXBehaviour[] behaviours;

    private GameObject spawnedGraphic;

    public void Start()
    {
        if (!settings.effectObject) return;

        spawnedGraphic = Instantiate(settings.effectObject, transform);
        SetEffectColour();

        foreach (MPFXBehaviour stencil in behaviours)
        {
            stencil.SetUp(spawnedGraphic);
        }
    }

    public void Update()
    {
        int behavioursComplete = 0;

        foreach (MPFXBehaviour stencil in behaviours)
        {
            if (stencil.UpdatePFX())
            {
                ++behavioursComplete;
            }
        }

        if (behavioursComplete == behaviours.Length)
        {
            EndPFX();
        }
    }

    private void EndPFX()
    {
        foreach (MPFXBehaviour stencil in behaviours)
        {
            stencil.End();
        }

        Destroy(spawnedGraphic);
    }

    private void SetEffectColour()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mesh in meshes)
        {
            mesh.material = mpfxMaterial;

            //These magic strings are awful, but are needed to workaround a bug in our verion of Unity
            //These allow for the emissive colour to be altererd at runtime, which is broken when trying
            //to alter these through the built in shader variables.
            mesh.material.SetColor("Color33632292", settings.effectColor);
            mesh.material.SetColor("ColorA9688267", settings.effectEmissive);
        }
    }
}
 