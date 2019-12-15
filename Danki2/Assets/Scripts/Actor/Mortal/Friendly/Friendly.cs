public abstract class Friendly : Mortal
{
    public abstract AI AI { get; }

    protected override void Update()
    {
        base.Update();

        AI.Act();
    }
}
