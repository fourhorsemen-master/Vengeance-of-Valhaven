public abstract class AIElement
{
    private float[] _args;
    public float[] Args
    {
        get { return _args; }
        set
        {
            _args = value;
            Initialize();
        }
    }

    public virtual void Initialize() { }
}
