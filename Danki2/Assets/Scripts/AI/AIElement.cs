public abstract class AIElement
{
    private float[] _args;

    public float[] Args
    {
        get { return _args; }
        set
        {
            _args = value;
            DeserializeArgs();
        }
    }

    public virtual void DeserializeArgs() { }

    public virtual void OnStart(Actor actor) { }
}
