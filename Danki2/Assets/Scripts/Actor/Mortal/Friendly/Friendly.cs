public abstract class Friendly : Mortal
{
    public abstract AI AI { get; }

    protected override void Act()
    {
        this.AI.Act();
    }
}
