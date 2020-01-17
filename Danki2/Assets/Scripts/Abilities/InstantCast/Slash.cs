using UnityEngine;

public class Slash : InstantCast
{
    public Slash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Debug.Log("Instant casting: Slash");
    }
}
