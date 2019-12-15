public class AlwaysAdvance<T> : Planner<T> where T : Actor
{
    public Agenda Plan(T actor, Agenda previousAgenda)
    {
        return new Agenda
        {
            { AIAction.Advance, true }
        };
    }
}
