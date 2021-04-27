using UnityEngine;

public class Wraith : Enemy
{
    public Subject SwipeSubject { get; } = new Subject();

    public override ActorType Type => ActorType.Wraith;

    public void Spine(Actor target)
    {
        InstantCastService.TryCast(
            AbilityReference.Spine,
            target.transform.position,
            target.Centre,
            target
        );
    }

    public void GuidedOrb(Actor target)
    {
        InstantCastService.TryCast(
            AbilityReference.GuidedOrb,
            target.transform.position,
            target.Centre,
            target
        );
    }

    public void Swipe()
    {
        InstantCastService.TryCast(
            AbilityReference.WraithSwipe,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
        SwipeSubject.Next();
    }

    public void Blink(Vector3 target)
    {
        InstantCastService.TryCast(
            AbilityReference.Blink,
            target,
            target + Height * Vector3.up
        );
    }
}
