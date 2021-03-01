using UnityEngine;

public class Bear : Enemy
{
    public override ActorType Type => ActorType.Bear;

    public void Swipe()
    {
        InstantCastService.TryCast(
            AbilityReference.Swipe,
            GetTargetPosition(transform.position),
            GetTargetPosition(Centre)
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
            GetTargetPosition(transform.position),
            GetTargetPosition(Centre)
        );
    }

    public void Cleave()
    {
        InstantCastService.TryCast(
            AbilityReference.Cleave,
            GetTargetPosition(transform.position),
            GetTargetPosition(Centre)
        );
    }

    private Vector3 GetTargetPosition(Vector3 origin) => origin + transform.forward;
}
