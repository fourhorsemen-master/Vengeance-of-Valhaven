using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Effects
{
    public class Dash : Effect
    {
        private readonly float speedMultiplier;
        private readonly float slowDuration;
        private readonly float slowMultiplier;

        public Dash(
            float duration,
            float speedMultiplier,
            float slowDuration,
            float slowMultiplier
        )
            : base(duration)
        {
            this.speedMultiplier = speedMultiplier;
            this.slowDuration = slowDuration;
            this.slowMultiplier = slowMultiplier;
        }

        protected override void FinishAction(Actor actor)
        {
            actor.AddEffect(
                new Slow(this.slowDuration, this.slowMultiplier)
            );
        }

        protected override void UpdateAction(Actor actor, float deltaTime)
        {
            // actor.SetStat(
            //     Stats.Speed,
            //     actor.GetStat(Stats.Speed) * this.speedMultiplier
            // );
        }
    }
}
