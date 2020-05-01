[Planner("No Op Planner", new string[0])]
public class NoOpPlanner : Planner
{
    public override void Setup(AI ai)
    {
    }

    public override Agenda Plan(Actor actor, Agenda previousAgenda)
    {
        return new Agenda();
    }
}
