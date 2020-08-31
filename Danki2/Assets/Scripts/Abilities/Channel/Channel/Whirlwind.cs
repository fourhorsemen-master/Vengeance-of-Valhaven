using System;
using UnityEngine;

[Ability(AbilityReference.Whirlwind, new[]{"Cross-Step"})]
public class Whirlwind : Channel
{
    private const float spinRange = 2;
    private const float spinDamageInterval = 0.35f;
    private const float spinDamageStartDelay = 0.1f;
    private const float selfSlowMultiplier = 0.5f;
    private const float finishRange = 3;

    private bool hasHitActor = false;
    private WhirlwindObject whirlwindObject;

    private Guid slowEffectId;
    private bool slowEffect = false;

    private Repeater repeater;

    public override float Duration => 2f;

    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.None;

    public Whirlwind(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Start(Vector3 target)
    {
        MultiplicativeStatModification slow = new Slow(selfSlowMultiplier);
        repeater = new Repeater(spinDamageInterval, () => AOE(spinRange, a => DealPrimaryDamage(a)), spinDamageStartDelay);

        if (!HasBonus("Cross-Step"))
        {
            slowEffect = true;
            slowEffectId = Owner.EffectManager.AddPassiveEffect(slow);
        }

        whirlwindObject = WhirlwindObject.Create(Owner.transform);
    }

    public override void Continue(Vector3 target)
    {
        repeater.Update();
    }

    public override void Cancel(Vector3 target)
    {
        if (!hasHitActor) SuccessFeedbackSubject.Next(false);

        if(slowEffect) Owner.EffectManager.RemovePassiveEffect(slowEffectId);

        whirlwindObject.DestroyWhirlwind();
    }

    public override void End(Vector3 target)
    {
        AOE(finishRange, a => DealSecondaryDamage(a));

        if (!hasHitActor) SuccessFeedbackSubject.Next(false);

        if(slowEffect) Owner.EffectManager.RemovePassiveEffect(slowEffectId);

        whirlwindObject.DestroyWhirlwind();
    }

    private void AOE(float radius, Action<Actor> damageAction)
    {
        bool actorsHit = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Cylinder, radius, Owner.transform.position, Quaternion.identity)
            .Where(actor => actor.Opposes(Owner))
            .ForEach(actor =>
            {
                damageAction(actor);
                actorsHit = true;
            });

        if (actorsHit)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
            hasHitActor = true;
            SuccessFeedbackSubject.Next(true);
        }
    }
}
