public interface Planner<T> where T : Actor
{
    Agenda Plan(T actor, Agenda previousAgenda);
}
