[Planner("No Op Planner", new string[0])]
public class NoOpPlanner : Planner
{
    public override Agenda Plan(AI ai, Actor actor, Agenda previousAgenda)
    {
        return new Agenda();
    }
}
