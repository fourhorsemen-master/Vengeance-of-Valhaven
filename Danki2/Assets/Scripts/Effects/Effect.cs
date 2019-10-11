using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public abstract class Effect
    {
        private float remainingDuration;
        public Effect(float duration)
        {
            remainingDuration = duration;
        }

        protected abstract void UpdateAction(Actor actor);

        protected abstract void FinishAction(Actor actor);

        public bool Update(Actor actor)
        {
            remainingDuration -= Time.deltaTime;
            this.UpdateAction(actor);
            if (remainingDuration < 0f)
            {
                this.FinishAction(actor);
                return true;
            }
            return false;
        }
    }
}
