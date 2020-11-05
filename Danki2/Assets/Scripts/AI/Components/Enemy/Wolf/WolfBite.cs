public class WolfBite : IAiComponent
{
    private readonly Wolf wolf;

    public WolfBite(Wolf wolf)
    {
        this.wolf = wolf;
    }

    public void Enter() => wolf.Bite();
    public void Exit() {}
    public void Update() {}
}
