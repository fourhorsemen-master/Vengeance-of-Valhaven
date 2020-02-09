using System;
using UnityEngine;

[Behaviour("Wolf Attack", new string[] { "Bite Cooldown", "Pounce Cooldown", "Pounce Min Range" }, new AIAction[] { AIAction.Attack })]
public class WolfAttack : Behaviour
{
    private float _biteCastTime = 0.5f;

    private float _biteTotalCooldown;
    private float _pounceTotalCooldown;

    private float _biteRemainingCooldown = 0f;
    private float _pounceRemainingCooldown = 0f;

    private float _pounceMinimumRange;

    public override void Initialize()
    {
        _biteTotalCooldown = Args[0];
        _pounceTotalCooldown = Args[1];
        _pounceMinimumRange = Args[2];
    }

    public override void Behave(AI ai, Actor actor)
    {
        Wolf wolf = (Wolf)actor;

        _biteRemainingCooldown -= Time.deltaTime;
        _pounceRemainingCooldown -= Time.deltaTime;

        if (wolf.RemainingChannelDuration > 0)
        {
            return;
        }

        if (!ai.Target)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(
            wolf.transform.position,
            ai.Target.transform.position
        );

        if (distanceToTarget < Bite.Range)
        {
            if (_biteRemainingCooldown <= 0)
            {
                wolf.ShowWarning(_biteCastTime);
                wolf.WaitAndCast(_biteCastTime, () =>
                    new Bite(new AbilityContext(wolf, ai.Target.transform.position))
                );

                _biteRemainingCooldown = _biteTotalCooldown;
            }
        }

        if (distanceToTarget < Pounce.Range
            && _pounceMinimumRange < distanceToTarget)
        {
            if (_pounceRemainingCooldown <= 0)
            {
                wolf.ShowWarning(0.3f);
                Channel pounce = new Pounce(new AbilityContext(wolf, ai.Target.transform.position));
                wolf.StartChannel(pounce);
                _biteRemainingCooldown = _biteTotalCooldown;
                _pounceRemainingCooldown = _pounceTotalCooldown;
            }
        }

    }
}
