public abstract class Enemy : Mortal
{
    public abstract AI AI { get; }

    protected override void Act()
    {
        this.AI.Act();
    }
}
