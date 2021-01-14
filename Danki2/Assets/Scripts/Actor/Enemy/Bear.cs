using System;
using UnityEngine;

public class Bear : Enemy
{
    public override ActorType Type => ActorType.Bear;

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
        throw new NotImplementedException();
    }

    public void Maul()
    {
        throw new NotImplementedException();
    }

    public void Cleave()
    {
        throw new NotImplementedException();
    }

    private Vector3 GetSwipeTargetPosition(Vector3 origin) => origin + transform.forward;
}
