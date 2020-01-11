public abstract class Behaviour
{
    private float[] _args;
    public float[] Args
    {
        get { return _args; }
        set
        {
            _args = value;
            Initialise();
        }
    }

    protected virtual void Initialise() { }

    public abstract void Behave(Actor actor);
}
