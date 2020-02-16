﻿using UnityEngine;

class DaggerThrow : InstantCast
{
    private static readonly float _daggerSpeed = 20f;
    private static readonly Vector3 _positionTransform = new Vector3(0, 1.25f, 0);

    public DaggerThrow(AbilityContext context) : base(context)
    {

    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position + _positionTransform;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        DaggerObject.Fire(Context.Owner, OnCollision, _daggerSpeed, position, rotation);
    }

    protected void OnCollision(GameObject gameObject)
    {
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                return;
            }

            var strength = Context.Owner.GetStat(Stat.Strength);
            actor.ModifyHealth(-strength/2);
            actor.AddEffect(new DOT(2), 5);
        }
    }
}