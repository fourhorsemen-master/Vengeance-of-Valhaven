using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Roll)]
public class Roll : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0, 0);
    public static readonly Dictionary<OrbType, int> GeneratedOrbs = new Dictionary<OrbType, int>();
    public const OrbType AbilityOrbType = OrbType.Aggression;
    public const string Tooltip = "Deals {PRIMARY_DAMAGE} damage.";
    public const string DisplayName = "Roll";

    private const float RollSpeedMultiplier = 4f;
    private const float RollDuration = 0.15f;

    public Roll(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = position.y;
        Vector3 direction = target - position;

        Owner.MovementManager.LockMovement(
            RollDuration,
            Owner.GetStat(Stat.Speed) * RollSpeedMultiplier,
            direction,
            direction
        );

        RollObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);
    }
}
