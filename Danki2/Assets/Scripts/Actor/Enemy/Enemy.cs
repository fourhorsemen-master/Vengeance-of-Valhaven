public abstract class Enemy : Actor
{
    protected override void Start()
    {
        base.Start();

        this.gameObject.tag = Tags.Enemy;
    }
}
