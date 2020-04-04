public abstract class InstantCast : Ability
{
    public override AbilityType AbilityType => AbilityType.InstantCast;

    public abstract void Cast(AbilityContext context);
}
