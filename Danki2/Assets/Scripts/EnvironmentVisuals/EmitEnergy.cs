using System.Collections.Generic;
using UnityEngine;

public class EmitEnergy : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> meshes = null;

    [SerializeField]
    private float intensity = 0;

    [SerializeField]
    private bool updateBaseColour = false;

    private void Start()
    {
        Color colour = VisualSettings.Instance.EnergyColour * Mathf.Pow(2, intensity);

        meshes.ForEach(m =>  m.material.SetEmissiveColour(colour));

        if (updateBaseColour)
        {
            meshes.ForEach(m => m.material.SetColour(VisualSettings.Instance.EnergyColour));
        }
    }
}
