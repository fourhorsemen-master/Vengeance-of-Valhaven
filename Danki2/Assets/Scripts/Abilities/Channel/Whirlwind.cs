using UnityEngine;

public class Whirlwind : Channel
{
    public Whirlwind(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 3f;

    public override void Cancel()
    {
        Debug.Log("Cancelling channel.");
    }

    public override void Continue()
    {
        Debug.Log("Continuing channel.");
    }

    public override void End()
    {
        Debug.Log("Finishing channel.");
    }

    public override void Start()
    {
        Debug.Log("Starting channel.");
    }
}
