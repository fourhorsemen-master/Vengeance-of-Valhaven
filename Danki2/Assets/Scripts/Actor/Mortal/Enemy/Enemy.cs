public abstract class Enemy : Mortal
{
    public abstract AI AI { get; }

    protected override void Update()
    {
        base.Update();

        AI.Act();
    }
}
