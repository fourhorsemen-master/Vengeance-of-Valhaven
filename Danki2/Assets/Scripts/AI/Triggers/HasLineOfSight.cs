using UnityEngine;

public class HasLineOfSight : StateMachineTrigger
{
    private readonly Transform fromTransform;
    private readonly Transform toTransform;

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
        return Physics.Raycast(from, to - from, Vector3.Distance(from, to));
    }
}
