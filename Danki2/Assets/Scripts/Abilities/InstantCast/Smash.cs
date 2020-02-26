using UnityEngine;

public class Smash : InstantCast
{
    public Smash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Debug.Log("casting smash");
    }
}
