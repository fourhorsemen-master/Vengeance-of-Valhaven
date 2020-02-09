﻿using UnityEngine;

public class PounceOld : Channel
{
    public PounceOld(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 0.8f;
    private float _initialRootDuration = 0.3f;
    private float _initialRootTimer = 0f;
    private bool _pounceStarted = false;
    private float _finalRootDuration = 0.3f;

    public static float Range => 10f;

    public override void Start()
    {
        Debug.Log("Pounce start");
        Context.Owner.Root(
            Duration,
            (Context.TargetPosition - Context.Owner.transform.position)
        );

        _initialRootTimer = _initialRootDuration;
    }

    public override void Continue()
    {
        Debug.Log("Pounce continuing");
        _initialRootTimer -= Time.deltaTime;

        if (_initialRootTimer >= 0f || _pounceStarted)
        {
            return;
        }

        _pounceStarted = true;

        var targetPosition = Context.TargetPosition;
        targetPosition.y = Context.Owner.transform.position.y;

        Context.Owner.LockMovement(
            (Duration - _initialRootDuration),
            Context.Owner.GetStat(Stat.Speed) * 3f,
            (targetPosition - Context.Owner.transform.position),
            @override: true
        );
    }

    public override void Cancel()
    {
        Debug.Log("Pounce cancelled");
    }

    public override void End()
    {

        Actor owner = Context.Owner;

        float damage = owner.GetStat(Stat.Strength);

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            owner.transform.position,
            Quaternion.LookRotation(owner.transform.forward)
        ).ForEach(actor =>
        {
            Debug.Log(actor.tag);
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
            }
        });

        owner.Root(_finalRootDuration, owner.transform.forward, true);
    }
}
