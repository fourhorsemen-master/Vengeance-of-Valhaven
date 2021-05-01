public class Bear : Enemy
{
    public override ActorType Type => ActorType.Bear;

    public Subject MaulAndSwipeSubject { get; set; } = new Subject();

    public Subject CleaveSubject { get; set; } = new Subject();

    public void Swipe()
    {
        InstantCastService.TryCast(
            AbilityReference.Swipe,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );

        MaulAndSwipeSubject.Next();
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

        MaulAndSwipeSubject.Next();
    }

    public void Cleave()
    {
        InstantCastService.TryCast(
            AbilityReference.Cleave,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );

        CleaveSubject.Next();
    }
}
