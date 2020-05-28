﻿using UnityEngine;

[Ability(AbilityReference.Sprint)]
public class Sprint : Cast
{
    protected override float CastTime => 1;

    private const int SpeedModification = 3;
    private const float SprintDuration = 5;

    public Sprint(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {
        Owner.EffectManager.AddActiveEffect(new SpeedModification(SpeedModification), SprintDuration);
    }
}
