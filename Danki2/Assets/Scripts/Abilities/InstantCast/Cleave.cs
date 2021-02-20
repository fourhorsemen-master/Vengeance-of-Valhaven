using UnityEngine;

[Ability(AbilityReference.Cleave)]
public class Cleave : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    public Cleave(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        CleaveObject.Create(Owner.AbilitySource, castRotation);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge180, Range, Owner.CollisionTemplateSource, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        PlayStartEvent();
        SlashObject.Create(Owner.AbilitySource, castRotation);

        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        var shakeIntensity = hasDealtDamage ? ShakeIntensity.High : ShakeIntensity.Low;
        CustomCamera.Instance.AddShake(shakeIntensity);
    }
}
