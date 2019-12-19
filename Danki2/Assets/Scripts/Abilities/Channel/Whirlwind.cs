using UnityEngine;

public class Whirlwind : Channel
{
    public Whirlwind(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 3f;

    public override void Start()
    {
        Debug.Log("Whirlwind start");
    }

    public override void Continue()
    {
        Debug.Log("Whirlwind continuing");
    }

    public override void Cancel()
    {
        Debug.Log("Whirlwind cancelled");
    }

    public override void End()
    {
        Debug.Log("Whirlwind end");
    }
}
