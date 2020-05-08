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

    public virtual int ProcessOutgoingDamage(int damage)
    {
        return damage;
    }

    public virtual int ProcessIncomingDamage(int damage)
    {
        return damage;
    }

    public virtual int ProcessIncomingHeal(int healing)
    {
        return healing;
    }
}
