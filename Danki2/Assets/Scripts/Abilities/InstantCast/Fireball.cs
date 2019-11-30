using UnityEngine;

public class Fireball : InstantCast
{
    public Fireball(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Debug.Log("Cast Fireball");
    }
}
