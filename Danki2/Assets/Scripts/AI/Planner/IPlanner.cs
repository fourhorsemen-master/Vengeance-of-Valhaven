public interface IPlanner
{
    Agenda Plan(Actor actor, Agenda previousAgenda);
}
