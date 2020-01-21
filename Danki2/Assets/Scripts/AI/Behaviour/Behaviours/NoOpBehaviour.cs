[Behaviour(
    "No Op Behaviour",
    new string[0],
    new AIAction[] {
        AIAction.FindTarget,
        AIAction.Attack,
        AIAction.Defend,
        AIAction.Advance,
        AIAction.Retreat,
        AIAction.Evade
    }
)]
public class NoOpBehaviour : Behaviour
{
    public override void Behave(AI ai, Actor actor)
    {
    }
}
