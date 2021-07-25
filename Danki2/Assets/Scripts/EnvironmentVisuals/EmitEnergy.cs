using System.Collections.Generic;
using UnityEngine;

public class EmitEnergy : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> meshes = null;

    [SerializeField]
    private int intensity = 0;

    private void Start()
    {
        Color colour = VisualSettings.Instance.EnergyColour * Mathf.Pow(2, intensity);

        meshes.ForEach(m =>  m.material.SetEmissiveColour(colour));
    }
}
