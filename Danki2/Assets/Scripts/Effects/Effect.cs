using UnityEngine;

public abstract class Effect
{
    public string GetDisplayName() => EffectLookup.Instance.GetDisplayName(GetType());
    public Sprite GetSprite() => EffectLookup.Instance.GetSprite(GetType());
    
    public virtual void Start(Actor actor) { }

    public virtual void Update(Actor actor) { }

    public virtual void Finish(Actor actor) { }

    public virtual bool Stuns => false;
    public virtual bool Roots => false;

    public virtual int GetLinearStatModifier(Stat stat) => 0;
    public virtual float GetMultiplicativeStatModifier(Stat stat) => 1;

    public virtual int GetLinearOutgoingDamageModifier() => 0;
    public virtual float GetMultiplicativeOutgoingDamageModifier() => 1;

    public virtual int GetLinearIncomingDamageModifier() => 0;
    public virtual float GetMultiplicativeIncomingDamageModifier() => 1;

    public virtual int GetLinearIncomingHealModifier() => 0;
    public virtual float GetMultiplicativeIncomingHealModifier() => 1;
}
