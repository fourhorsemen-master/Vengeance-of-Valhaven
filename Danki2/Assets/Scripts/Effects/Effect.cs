public abstract class Effect
{
    public virtual void Start(Actor actor)
    {

    }

    public virtual void Update(Actor actor)
    {

    }

    public virtual void Finish(Actor actor)
    {

    }

    public virtual float ProcessStat(Stat stat, float value)
    {
        return value;
    }
}
