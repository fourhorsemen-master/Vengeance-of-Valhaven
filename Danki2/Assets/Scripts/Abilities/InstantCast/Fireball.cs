using UnityEngine;

public class Fireball : InstantCast
{
    public Fireball(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position;
        Quaternion rotation = Quaternion.FromToRotation(position, Context.TargetPosition);
        FireballObject.Fire(position, rotation);
    }
}
