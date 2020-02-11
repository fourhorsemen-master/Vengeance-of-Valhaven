[Behaviour("Retreat From Target", new string[0], new AIAction[] { AIAction.Retreat })]
public class RetreatFromTarget : Behaviour
{
    public override void Behave(Actor actor)
    {
        if (actor.Target)
        {
            actor.MoveToward((2 * actor.transform.position) - actor.Target.transform.position);
        }
    }
}
