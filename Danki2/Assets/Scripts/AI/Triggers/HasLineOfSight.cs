using UnityEngine;

public class HasLineOfSight : StateMachineTrigger
{
    private readonly Transform fromTransform;
    private readonly Transform toTransform;

    private readonly int layerMask = LayerUtils.GetInvertedLayerMask(LayerUtils.GetLayerMask(Layer.Actors, Layer.ClothSim));

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
