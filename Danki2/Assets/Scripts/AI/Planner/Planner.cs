public abstract class Planner : AIElement
{
    public abstract void Setup(Actor actor);

    public abstract Agenda Plan(Actor actor, Agenda previousAgenda);
}
