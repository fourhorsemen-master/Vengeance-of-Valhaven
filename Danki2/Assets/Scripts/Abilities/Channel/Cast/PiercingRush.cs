using UnityEngine;

[Ability(AbilityReference.PiercingRush, new[] { "Daze", "Jetstream" })]
public class PiercingRush : Cast
{
    protected override float CastTime => 1;

    private const int SpeedModification = 3;
    private const float SprintDuration = 5;

    public PiercingRush(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target) => End();

    public override void End(Actor actor) => End();

    private void End()
    {
        LinearStatModification speedModification = new LinearStatModification(Stat.Speed, SpeedModification);
        Owner.EffectManager.AddActiveEffect(speedModification, SprintDuration);
        SuccessFeedbackSubject.Next(true);
    }
}
