using System;
using UnityEngine;

public class Bear : Enemy
{
    public override ActorType Type => ActorType.Bear;

    public bool IsTelegraphing { get; private set; }

    public Bear()
    {
        OnTelegraph.Subscribe(duration =>
        {
            IsTelegraphing = true;
            this.WaitAndAct(duration, () => IsTelegraphing = false);
        });
    }

    public void Swipe()
    {
        InstantCastService.TryCast(
            AbilityReference.Swipe,
            GetSwipeTargetPosition(transform.position),
            GetSwipeTargetPosition(Centre)
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
            GetSwipeTargetPosition(transform.position),
            GetSwipeTargetPosition(Centre)
        );
    }

    public void Cleave()
    {
        InstantCastService.TryCast(
            AbilityReference.Slash, // TODO: replace slash with cleave
            GetSwipeTargetPosition(transform.position),
            GetSwipeTargetPosition(Centre)
        );
    }

    private Vector3 GetSwipeTargetPosition(Vector3 origin) => origin + transform.forward;
}
