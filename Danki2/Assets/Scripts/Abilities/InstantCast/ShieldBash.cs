using UnityEngine;

public class ShieldBash : InstantCast
{
    public ShieldBash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Debug.Log("Casting ShieldBash");
    }
}
