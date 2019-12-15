public abstract class Passive : Actor
{
    public abstract AI AI { get; }

    protected override void Update()
    {
        base.Update();

        AI.Act();
    }
}
