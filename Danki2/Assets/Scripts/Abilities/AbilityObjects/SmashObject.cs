using UnityEngine;

public class SmashObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private MeshRenderer outerMeshRenderer = null;

    [SerializeField]
    private MeshRenderer innerMeshRenderer = null;

    private Color desiredColour = default;

    public static void Create(Vector3 position, float scaleFactor = 1f, Quaternion rotation = default, Color colour = default)
    {
        SmashObject prefab = AbilityObjectPrefabLookup.Instance.SmashObjectPrefab;
        SmashObject smashObject = Instantiate(
            prefab,
            position,
            rotation.Equals(default) ? Quaternion.identity : rotation
        );
        smashObject.gameObject.transform.localScale *= scaleFactor;
        smashObject.desiredColour = colour;
    }

    protected override void Start()
    {
        base.Start();
        
        if (!desiredColour.Equals(default))
        {
            Color lighterColour = Color.Lerp(desiredColour, Color.white, 0.5f);

            outerMeshRenderer.material.SetEmissiveColour(lighterColour);
            innerMeshRenderer.material.SetEmissiveColour(desiredColour);
        }
    }
}
