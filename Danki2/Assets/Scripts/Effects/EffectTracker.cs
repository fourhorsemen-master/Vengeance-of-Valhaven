using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Effects
{
    public class EffectTracker
    {
        private List<Effect> _effects;
        private readonly Actor _actor;

        public EffectTracker(Actor actor)
        {
            _effects = new List<Effect>();
            _actor = actor;
        }
        
        public void ProcessEffects()
        {
            _effects = _effects.FindAll(
                effect => {
                    effect.Update(_actor);
                    return !effect.Expired;
                }    
            );
        }

        public void AddEffect(Effect effect)
        {
            _effects.Add(effect);
        }
    }
}
