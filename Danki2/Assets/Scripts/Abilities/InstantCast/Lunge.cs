using UnityEngine;

public class Lunge : InstantCast
{
    public Lunge(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Debug.Log("casting lunge");
    }
}
