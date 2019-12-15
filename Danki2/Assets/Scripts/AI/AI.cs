public abstract class AI
{
    public abstract void Act();
}

public class AI<T> : AI where T : Actor
{
    private readonly T _actor;
    private readonly Planner<T> _planner;
    private readonly Personality<T> _personality;
    private Agenda _agenda;

    public AI(T actor, Planner<T> planner, Personality<T> personality)
    {
        _actor = actor;
        _planner = planner;
        _personality = personality;
        _agenda = new Agenda();
    }

    public override void Act()
    {
        _agenda = _planner.Plan(_actor, _agenda);

        foreach (AIAction key in _agenda.Keys)
        {
            if (_agenda[key] && _personality.TryGetValue(key, out var behaviour))
            {
                behaviour.Behave(_actor);
            }
        }
    }
}