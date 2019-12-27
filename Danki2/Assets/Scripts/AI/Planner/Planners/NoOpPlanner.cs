public class NoOpPlanner : Planner
{
    public override Agenda Plan(Actor actor, Agenda previousAgenda)
    {
        return new Agenda();
    }
}
