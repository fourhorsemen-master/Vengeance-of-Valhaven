using UnityEngine;

[Ability(AbilityReference.Sprint)]
public class Sprint : Cast
{
    protected override float CastTime => 1;

    private const int SpeedModification = 3;
    private const float SprintDuration = 5;
    
    private readonly Subject onCastCancelled = new Subject();
    private readonly Subject onCastEnd = new Subject();

    public Sprint(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    protected override void Start() => SprintObject.Create(Owner.transform, onCastCancelled, onCastEnd);

    protected override void Cancel() => onCastCancelled.Next();

    public override void End(Vector3 target) => End();

    public override void End(Actor actor) => End();

    private void End()
    {
        Owner.EffectManager.AddActiveEffect(new SpeedBuff(SpeedModification), SprintDuration);
        SuccessFeedbackSubject.Next(true);

        onCastEnd.Next();
        Owner.StartTrail(SprintDuration);
    }
}
