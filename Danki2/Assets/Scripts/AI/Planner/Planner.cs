public abstract class Planner : AIElement
{
    public abstract void Setup(AI ai, Actor actor);

    public abstract Agenda Plan(Actor actor, Agenda previousAgenda);
}
