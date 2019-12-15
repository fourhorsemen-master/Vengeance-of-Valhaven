using UnityEngine;

public class Fireball : InstantCast
{
    private static readonly float FIREBALL_SPEED = 5;
    private readonly AbilityContext context;

    public Fireball(AbilityContext context) : base(context)
    {
        this.context = context;
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(context.Owner, OnCollision, position, rotation, FIREBALL_SPEED);
    }

    public void OnCollision(GameObject gameObject)
    {

    }
}
