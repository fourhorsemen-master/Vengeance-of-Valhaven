using System;
using System.Collections.Generic;
using Agenda = System.Collections.Generic.Dictionary<Assets.Scripts.AI.AIAction, bool>;

namespace Assets.Scripts.AI
{
    public class AI
    {
        private readonly Actor actor;
        private readonly Func<Actor, Agenda> plan;
        private readonly Dictionary<AIAction, Action<Actor>> personality;
        private Agenda agenda;

        public AI(Actor actor, Func<Actor, Agenda> plan, Dictionary<AIAction, Action<Actor>> personality)
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
