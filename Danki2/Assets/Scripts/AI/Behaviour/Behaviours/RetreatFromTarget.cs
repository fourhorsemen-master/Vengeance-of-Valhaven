[Behaviour("Retreat From Target", new string[0], new AIAction[] { AIAction.Retreat })]
public class RetreatFromTarget : Behaviour
{
    public override void Behave(AI ai, Actor actor)
    {
        if (ai.Target)
        {
            actor.MoveToward((2 * actor.transform.position) - ai.Target.transform.position);
        }
    }
}
