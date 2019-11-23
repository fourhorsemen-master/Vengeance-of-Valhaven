using System;

public class Whirlwind : Channel
{
    public Whirlwind(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 3f;

    public override void Cancel()
    {
        throw new NotImplementedException();
    }

    public override void Continue()
    {
        throw new NotImplementedException();
    }

    public override void End()
    {
        throw new NotImplementedException();
    }

    public override void Start()
    {
        throw new NotImplementedException();
    }
}
