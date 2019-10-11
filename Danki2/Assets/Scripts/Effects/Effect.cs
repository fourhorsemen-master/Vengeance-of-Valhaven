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

        public bool Expired => this.remainingDuration < 0f;

        protected abstract void UpdateAction(Actor actor, float deltaTime);

        protected abstract void FinishAction(Actor actor);

        public void Update(Actor actor)
        {
            var deltaTime = Time.deltaTime;

            remainingDuration -= deltaTime;
            this.UpdateAction(actor, deltaTime);
            if (remainingDuration < 0f)
            {
                this.FinishAction(actor);
            }
        }
    }
}
