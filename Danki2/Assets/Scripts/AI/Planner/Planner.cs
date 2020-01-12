public abstract class Planner
{
    private float[] _args;
    public float[] Args
    {
        get { return _args; }
        set
        {
            _args = value;
            Initilize();
        }
    }

    public virtual void Initilize() { }

    public abstract Agenda Plan(Actor actor, Agenda previousAgenda);
}
