using UnityEngine;

public class HasLineOfSight : StateMachineTrigger
{
    private static readonly Layer[] layersToIgnore = {Layer.Actors, Layer.ClothSim, Layer.Abilities};
    
    private readonly Transform fromTransform;
    private readonly Transform toTransform;

    private readonly int layerMask = LayerUtils.GetInvertedLayerMask(LayerUtils.GetLayerMask(layersToIgnore));

    public HasLineOfSight(Transform fromTransform, Transform toTransform)
    {
        this.fromTransform = fromTransform;
        this.toTransform = toTransform;
    }

    public override void Activate() {}
    public override void Deactivate() {}

    public override bool Triggers()
    {
        Vector3 from = fromTransform.position;
        Vector3 to = toTransform.position;

        Vector3 direction = to - from;
        float maxDistance = Vector3.Distance(from, to);

        return !Physics.Raycast(from, direction, maxDistance, layerMask);
    }
}
