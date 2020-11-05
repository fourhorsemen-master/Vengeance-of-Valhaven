public abstract class Enemy : Actor
{
    public Subject<float> OnTelegraph { get; } = new Subject<float>();

    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Enemy;
    }
}
