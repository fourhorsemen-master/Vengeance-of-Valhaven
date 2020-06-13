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

    public virtual int GetAdditiveStatModifier(Stat stat)
    {
        return 0;
    }

    public virtual float GetMultiplicativeStatModifier(Stat stat)
    {
        return 1;
    }

    public virtual int GetAdditiveOutgoingDamageModifier()
    {
        return 0;
    }

    public virtual float GetMultiplicativeOutgoingDamageModifier()
    {
        return 1;
    }

    public virtual int GetAdditiveIncomingDamageModifier()
    {
        return 0;
    }
    
    public virtual float GetMultiplicativeIncomingDamageModifier()
    {
        return 1;
    }

    public virtual int GetAdditiveIncomingHealModifier()
    {
        return 0;
    }
    
    public virtual float GetMultiplicativeIncomingHealModifier()
    {
        return 1;
    }
}
