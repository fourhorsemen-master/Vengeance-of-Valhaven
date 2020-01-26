using UnityEngine;

public class Bite : InstantCast
{
    public Bite(AbilityContext context) : base(context)
    {
    }

    public static readonly float Range = 2f;

    public override void Cast()
    {
        Debug.Log("Instant casting: Bite");
        Context.Owner.LockMovement(0.5f, 0f, Context.TargetPosition - Context.Owner.transform.position);
    }
}
