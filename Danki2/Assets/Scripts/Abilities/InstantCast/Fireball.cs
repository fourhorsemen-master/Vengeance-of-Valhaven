using UnityEngine;

public class Fireball : InstantCast
{
    private static readonly float FIREBALL_SPEED = 5;

    public Fireball(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(position, rotation, FIREBALL_SPEED);
    }
}
