public class AI
{
    private readonly Actor _actor;
    private readonly Planner _planner;
    private readonly Personality _personality;
    private Agenda _agenda;

    public AI(Actor actor, Planner planner, Personality personality)
    {
        _actor = actor;
        _planner = planner;
        _personality = personality;
        _agenda = new Agenda();
    }

    public void Act()
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