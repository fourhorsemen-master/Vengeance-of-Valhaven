using UnityEngine;

[Behaviour("Wolf Attack", new string[] { "Bite Cooldown", "Pounce Cooldown", "Pounce Min Range" }, new AIAction[] { AIAction.Attack })]
public class WolfAttack : Behaviour
{    
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
        _biteRemainingCooldown -= Time.deltaTime;
        _pounceRemainingCooldown -= Time.deltaTime;

        if (actor.RemainingChannelDuration > 0)
        {
            return;
        }

        if (!ai.Target)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(
            actor.transform.position,
            ai.Target.transform.position
        );

        if (distanceToTarget < Bite.Range)
        {
            if (_biteRemainingCooldown <= 0)
            {
                new Bite(new AbilityContext(actor, ai.Target.transform.position)).Cast();
                _biteRemainingCooldown = _biteTotalCooldown;
            }
            return;
        }

        if (distanceToTarget < Pounce.Range
            && _pounceMinimumRange < distanceToTarget)
        {
            if (_pounceRemainingCooldown <= 0)
            {
                Channel pounce = new Pounce(new AbilityContext(actor, ai.Target.transform.position));
                actor.StartChannel(pounce);
                _biteRemainingCooldown = _biteTotalCooldown;
                _pounceRemainingCooldown = _pounceTotalCooldown;
            }
            return;
        }

    }
}
