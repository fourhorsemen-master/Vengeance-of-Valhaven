public class WolfHowl : IAiComponent
{
    private readonly Wolf wolf;

    public WolfHowl(Wolf wolf)
    {
        this.wolf = wolf;
    }

    public void Enter()
    {
        wolf.Howl();
    }

    public void Exit() {}

    public void Update() {}
}
