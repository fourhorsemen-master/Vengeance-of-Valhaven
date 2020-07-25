using UnityEngine;

[Ability(AbilityReference.Bandage, new []{"Perseverance"})]
public class Bandage : Channel
{
    private const float HealInterval = 1f;
    private const float HealStartDelay = 1f;

    private Repeater repeater;
    private bool hasHealed = false;
    
    public override float Duration => 5f;
    
    public Bandage(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Start(Vector3 target)
    {
        repeater = new Repeater(HealInterval, Heal, HealStartDelay);
    }

    public override void Continue(Vector3 target)
    {
        repeater.Update();
    }

    public override void Cancel(Vector3 target) => End();
    
    public override void End(Vector3 target) => End();

    private void Heal()
    {
        base.Heal();

        if (hasHealed) return;
        SuccessFeedbackSubject.Next(true);
        hasHealed = true;
    }

    private void End()
    {
        if (!hasHealed) SuccessFeedbackSubject.Next(false);
    }
}
