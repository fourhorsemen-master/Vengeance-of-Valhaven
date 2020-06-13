using UnityEngine;

[Ability(AbilityReference.Hook)]
public class Hook : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    private const float knockBackDuration = 0.5f;
    private const float knockBackSpeed = 5f;

    public Hook(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        
    }
}
