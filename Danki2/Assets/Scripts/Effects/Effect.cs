using UnityEngine;

public abstract class Effect
{
    private float _remainingDuration;

    public Effect(float duration)
    {
        _remainingDuration = duration;
    }

    public bool Expired => _remainingDuration < 0f;

    protected abstract void UpdateAction(Actor actor, float deltaTime);

    protected abstract void FinishAction(Actor actor);

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
