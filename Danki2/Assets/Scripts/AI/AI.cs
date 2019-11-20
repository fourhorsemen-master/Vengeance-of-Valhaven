using System;
using System.Collections.Generic;

public abstract class AI
{
    public abstract void Act();
}

public class AI<T> : AI where T : Actor
{
    private readonly T _actor;
    private readonly Func<T, Agenda, Agenda> _plan;
    private readonly Dictionary<AIAction, Action<T>> _personality;
    private Agenda _agenda;

    public AI(T actor, Func<T, Agenda, Agenda> plan, Personality<T> personality)
    {
        _actor = actor;
        _plan = plan;
        _personality = personality;
        _agenda = new Agenda();
    }

    public override void Act()
    {
        _agenda = _plan(_actor, _agenda);

        foreach (AIAction key in _agenda.Keys)
        {
            if (_agenda[key] && _personality.TryGetValue(key, out var behaviour))
            {
                behaviour(_actor);
            }
        }
    }
}