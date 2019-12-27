public abstract class Planner
{
    public virtual void Initilize(float[] args) { }
    public abstract Agenda Plan(Actor actor, Agenda previousAgenda);
}
