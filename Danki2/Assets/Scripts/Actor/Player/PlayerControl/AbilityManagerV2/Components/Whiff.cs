internal class Whiff : IStateMachineComponent
{
    private Player player;

    public Whiff(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Whiff();
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}