using UnityEngine;

public class DistanceWithin : StateMachineTrigger
{
    private readonly Actor actor1;
    private readonly Actor actor2;
    private readonly float minDistance;
    private readonly float maxDistance;

    public DistanceWithin(Actor actor1, Actor actor2, float minDistance, float maxDistance)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
    }

    public override void Activate() {}

    public override void Deactivate() {}

    public override bool Triggers()
    {
        float distance = Vector3.Distance(actor1.transform.position, actor2.transform.position);
        return minDistance < distance && distance < maxDistance;
    }
}
