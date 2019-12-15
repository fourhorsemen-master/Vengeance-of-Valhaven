public class AlwaysAdvance<T> : IPlanner<T> where T : Actor
{
    public Agenda Plan(T actor, Agenda previousAgenda)
    {
        return new Agenda
        {
            { AIAction.Advance, true }
        };
    }
}
