[Planner("Always Advance", new string[0])]
public class AlwaysAdvance : Planner
{
    public override Agenda Plan(Actor actor, Agenda previousAgenda)
    {
        return new Agenda
        {
            { AIAction.Advance, true }
        };
    }
}
