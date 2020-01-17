using UnityEngine;

public class Bite : InstantCast
{
    public Bite(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Debug.Log("Instant casting: Bite");
    }
}
