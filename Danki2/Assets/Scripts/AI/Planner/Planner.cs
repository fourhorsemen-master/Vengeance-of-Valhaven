public abstract class Planner : AIElement
{
    public abstract Agenda Plan(Actor actor, Agenda previousAgenda);
}
