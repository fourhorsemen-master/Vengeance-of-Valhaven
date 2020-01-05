[Behaviour(
    "No Op Behaviour",
    new AIAction[] {
        AIAction.FindTarget,
        AIAction.Attack,
        AIAction.Defend,
        AIAction.Advance,
        AIAction.Retreat,
        AIAction.Evade
    },
    new string[0]
)]
public class NoOpBehaviour : Behaviour
{
    public override void Behave(Actor actor)
    {
    }
}
