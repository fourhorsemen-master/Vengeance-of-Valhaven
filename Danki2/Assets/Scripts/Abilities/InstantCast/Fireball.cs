﻿using UnityEngine;

[Ability(AbilityReference.Fireball)]
public class Fireball : InstantCast
{
    private const float FireballSpeed = 5;

    public Fireball(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Quaternion rotation = Quaternion.LookRotation(target - Owner.Centre);
        FireballObject.Fire(Owner, OnCollision, FireballSpeed, Owner.Centre, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.High);

        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            DealPrimaryDamage(actor);
            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
