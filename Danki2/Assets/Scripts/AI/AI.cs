using System;
using System.Collections.Generic;
using Agenda = System.Collections.Generic.Dictionary<Assets.Scripts.AI.AIAction, bool>;

namespace Assets.Scripts.AI
{
    public class AI<T> : IAI where T : Actor
    {
        private readonly T actor;
        private readonly Func<T, Agenda> plan;
        private readonly Dictionary<AIAction, Action<T>> personality;
        private Agenda agenda;

        public AI(T actor, Func<T, Agenda> plan, Dictionary<AIAction, Action<T>> personality)
        {
            this.actor = actor;
            this.plan = plan;
            this.personality = personality;
            this.agenda = new Agenda();
        }

        public void Act()
        {
            this.agenda = this.plan(this.actor);

            foreach (AIAction key in this.agenda.Keys)
            {
                if (this.agenda[key] && this.personality.TryGetValue(key, out var behaviour))
                {
                    behaviour(this.actor);
                }
            }
        }
    }
}
