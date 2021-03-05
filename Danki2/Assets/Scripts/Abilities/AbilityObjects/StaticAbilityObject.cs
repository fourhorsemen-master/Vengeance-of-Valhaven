public abstract class StaticAbilityObject : AbilityObject
{
    public abstract float StickTime { get; }

    protected virtual void Start()
    {
        this.WaitAndAct(StickTime, () => Destroy(gameObject));
    }
}
