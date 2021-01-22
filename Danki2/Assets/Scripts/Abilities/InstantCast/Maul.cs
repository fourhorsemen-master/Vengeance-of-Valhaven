using UnityEngine;

[Ability(AbilityReference.Maul)]
public class Maul : InstantCast
{
    private const int BiteCount = 4;
    private const float BiteInterval = 0.4f;
    private const float BiteRange = 3f;
    private const float PauseDuration = 0.3f;

    public Maul(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        // TODO: make caster immune to CC for the duration of the ability
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;

        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(BiteCount * BiteInterval + PauseDuration);

        MaulObject maulObject = MaulObject.Create(Owner.AbilitySource);

        Direction direction = Direction.Left;

        Owner.ActOnInterval(BiteInterval, () => Bite(castDirection, ref direction, maulObject), 0f, BiteCount);

        Owner.WaitAndAct(BiteInterval * BiteCount * 2, maulObject.Destroy);
    }

    private void Bite(Vector3 castDirection, ref Direction direction, MaulObject maulObject)
    {
        Vector3 horizontalDirection = Vector3.Cross(castDirection, Vector3.up).normalized;
        int directionMultiplier = direction == Direction.Right ? 1 : -1;
        Vector3 randomisedcastDirection = castDirection.normalized + horizontalDirection * Random.Range(0.25f, 0.5f) * directionMultiplier;

        Quaternion castRotation = GetMeleeCastRotation(randomisedcastDirection);

        maulObject.Bite(castRotation);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge45, BiteRange, Owner.CollisionTemplateSource, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                actor.EffectManager.AddStack(StackingEffect.Slow);
                hasDealtDamage = true;
            });

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }

        direction = direction == Direction.Left ? Direction.Right : Direction.Left;

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}
