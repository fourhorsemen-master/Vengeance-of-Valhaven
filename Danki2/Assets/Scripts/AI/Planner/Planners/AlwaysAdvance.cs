public class AlwaysAdvance : IPlanner
{
    public Agenda Plan(Actor actor, Agenda previousAgenda)
    {
        return new Agenda
        {
            { AIAction.Advance, true }
        };
    }
}
