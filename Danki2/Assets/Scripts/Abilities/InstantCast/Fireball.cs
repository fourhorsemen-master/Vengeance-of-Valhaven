using UnityEngine;

public class Fireball : InstantCast
{
    public Fireball(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position;
        Vector3 target = Context.TargetPosition;
        FireballObject.Fire(position, target - position);
    }
}
