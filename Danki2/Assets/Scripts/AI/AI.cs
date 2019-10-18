using System;
using System.Collections.Generic;
using Agenda = System.Collections.Generic.Dictionary<Assets.Scripts.AI.AIAction, bool>;

namespace Assets.Scripts.AI
{
    public class AI<T> : IAI where T : Actor
    {
        private readonly T _actor;
        private readonly Func<T, Agenda> _plan;
        private readonly Dictionary<AIAction, Action<T>> _personality;
        private Agenda _agenda;

        public AI(T actor, Func<T, Agenda> plan, Dictionary<AIAction, Action<T>> personality)
        {
            _actor = actor;
            _plan = plan;
            _personality = personality;
            _agenda = new Agenda();
        }

        public void Act()
        {
            _agenda = _plan(_actor);

            foreach (AIAction key in _agenda.Keys)
            {
                if (_agenda[key] && _personality.TryGetValue(key, out var behaviour))
                {
                    behaviour(_actor);
                }
            }
        }
    }
}
