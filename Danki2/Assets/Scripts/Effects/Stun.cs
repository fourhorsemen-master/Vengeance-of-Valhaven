using UnityEngine;

public class Stun : Effect
{
    private readonly float _duration;

    public Stun(float duration)
    {
        _duration = duration;
    }

    public override void Start(Actor actor)
    {
        actor.LockMovement(
            _duration,
            0,
            Vector3.zero,
            @override: true
        );
    }
}
