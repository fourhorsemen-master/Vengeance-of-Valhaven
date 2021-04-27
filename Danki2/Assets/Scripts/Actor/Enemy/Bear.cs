public class Bear : Enemy
{
    public override ActorType Type => ActorType.Bear;

    public void Swipe()
    {
        InstantCastService.TryCast(
            AbilityReference.Swipe,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
    }

    public void Charge()
    {
        ChannelService.TryStartChannel(AbilityReference.BearCharge);
    }

    public void Maul()
    {
        InstantCastService.TryCast(
            AbilityReference.Maul,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
    }

    public void Cleave()
    {
        InstantCastService.TryCast(
            AbilityReference.Cleave,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
    }
}
