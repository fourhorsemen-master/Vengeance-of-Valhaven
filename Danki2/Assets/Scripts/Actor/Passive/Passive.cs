using Assets.Scripts.AI;

public abstract class Passive : Actor
{
    public abstract AI AI { get; }

    protected override void Act()
    {
        this.AI.Act();
    }
}
