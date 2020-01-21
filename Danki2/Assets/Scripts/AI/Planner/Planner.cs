public abstract class Planner : AIElement
{
    public abstract Agenda Plan(AI ai, Actor actor, Agenda previousAgenda);
}
