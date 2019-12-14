using UnityEngine;

public abstract class Effect
{
    private float _remainingDuration;

    public Effect(float duration)
    {
        _remainingDuration = duration;
    }

    public bool Expired => _remainingDuration < 0f;

    protected virtual void UpdateAction(Actor actor, float deltaTime)
    {

    }

    protected virtual void FinishAction(Actor actor)
    {

    }

    public virtual float ProcessStat(Stat stat, float value)
    {
        return value;
    }

    public void Update(Actor actor)
    {
        var deltaTime = Time.deltaTime;

        _remainingDuration -= deltaTime;
        this.UpdateAction(actor, deltaTime);
        if (_remainingDuration < 0f)
        {
            this.FinishAction(actor);
        }
    }
}
