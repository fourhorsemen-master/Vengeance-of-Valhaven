using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Effects
{
    public class Slow : Effect
    {
        private readonly float _slowMultiplier;

        public Slow(float duration, float slowMultiplier) : base(duration)
        {
            _slowMultiplier = slowMultiplier;
        }

        protected override void FinishAction(Actor actor)
        {
        }

        protected override void UpdateAction(Actor actor, float deltaTime)
        {
            // actor.SetStat(Stats.Speed, actor.GetStat(Stats.Speed) * this.slowMultiplier);
        }
    }
}
