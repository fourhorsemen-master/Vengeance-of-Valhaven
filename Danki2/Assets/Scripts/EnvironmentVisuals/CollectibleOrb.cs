using UnityEngine;
using UnityEngine.VFX;

public class CollectibleOrb : MonoBehaviour
{
    [SerializeField] private Color colour = default;
    [SerializeField] private int collectionAmount = 0;
    [SerializeField] private VisualEffect collectionVisual = null;
    [SerializeField] private MeshRenderer orb = null;
    [SerializeField] private Light glow = null;

    public void Start()
    {
        collectionVisual.SetInt("SpawnCount", collectionAmount);
        collectionVisual.SetVector4("Colour", colour);

        orb.material.SetColour(colour);
        orb.material.SetEmissiveColour(colour);

        glow.color = colour;
    }

    public void Destroy() => Destroy(gameObject);

    public void Collect()
    {
        collectionVisual.Play();
        orb.enabled = false;
        glow.enabled = false;
    }
}
