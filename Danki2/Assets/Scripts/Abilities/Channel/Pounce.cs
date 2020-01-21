using UnityEngine;

public class Pounce : Channel
{
    public Pounce(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 1f;

    public override void Start()
    {
        Debug.Log("Pounce start");
    }

    public override void Continue()
    {
        Debug.Log("Pounce continuing");
    }

    public override void Cancel()
    {
        Debug.Log("Pounce cancelled");
    }

    public override void End()
    {
        Debug.Log("Pounce end");
    }
}
