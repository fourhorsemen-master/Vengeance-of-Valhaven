using UnityEngine;

public class DistanceLessThan : AiTrigger
{
    private readonly Actor actor1;
    private readonly Actor actor2;
    private readonly float distance;

    public DistanceLessThan(Actor actor1, Actor actor2, float distance)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.distance = distance;
    }

    public override void Activate() {}

    public override void Deactivate() {}

    public override bool Triggers()
    {
        return Vector3.Distance(actor1.transform.position, actor2.transform.position) < distance;
    }
}
