using UnityEngine;

[Ability(AbilityReference.Execute, new[] { "Finishing Touch" })]
public class Execute : Cast
{
    private const float CastRange = 3f;
    private const float PauseDuration = 0.3f;

    public Execute(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses, float duration)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses, duration)
    {
    }

    public override void End(Actor target)
    {
        Owner.MovementManager.LookAt(target.transform.position);

        float distance = Vector3.Distance(Owner.transform.position, target.transform.position);

        if (distance > CastRange)
        {
            SuccessFeedbackSubject.Next(false);
        }

        Owner.MovementManager.Pause(PauseDuration);
    }
}
