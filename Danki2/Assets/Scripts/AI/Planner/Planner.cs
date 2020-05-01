public abstract class Planner : AIElement
{
    public abstract void Setup(AI ai);

    public abstract Agenda Plan(Actor actor, Agenda previousAgenda);
}
