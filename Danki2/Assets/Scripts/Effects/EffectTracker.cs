using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Effects
{
    public class EffectTracker
    {
        private List<Effect> Effects;
        private readonly Actor actor;

        public EffectTracker(Actor actor)
        {
            this.Effects = new List<Effect>();
            this.actor = actor;
        }
        
        public void ProcessEffects()
        {
            this.Effects = this.Effects.FindAll(
                effect => {
                    effect.Update(this.actor);
                    return !effect.Expired;
                }    
            );
        }

        public void AddEffect(Effect effect)
        {
            this.Effects.Add(effect);
        }
    }
}
