public interface IPlanner<T> where T : Actor
{
    Agenda Plan(T actor, Agenda previousAgenda);
}
