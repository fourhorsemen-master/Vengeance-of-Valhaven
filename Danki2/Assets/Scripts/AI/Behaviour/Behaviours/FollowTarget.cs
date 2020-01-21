[Behaviour("Follow Target", new string[0], new AIAction[] { AIAction.Advance })]
public class FollowTarget : Behaviour
{
    public override void Behave(AI ai, Actor actor)
    {
        if (ai.Target)
        {
            actor.MoveToward(ai.Target.transform.position);
        }
    }
}
