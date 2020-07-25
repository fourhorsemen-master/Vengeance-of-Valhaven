using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.IntimidatingShout)]
public class IntimidatingShout : InstantCast
{
    private const float Range = 3;
    private const int DefenceModification = -2;
    private const float Duration = 6;

    public IntimidatingShout(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        List<Actor> collidingActors = GetCollidingActors();
        SuccessFeedbackSubject.Next(collidingActors.Count > 0);
        collidingActors.ForEach(ReduceDefence);
    }

    private List<Actor> GetCollidingActors()
    {
        return CollisionTemplateManager.Instance
            .GetCollidingActors(CollisionTemplate.Cylinder, Range, Owner.transform.position)
            .Where(actor => Owner.Opposes(actor));
    }

    private void ReduceDefence(Actor actor)
    {
        actor.EffectManager.AddActiveEffect(new LinearStatModification(Stat.Defence, DefenceModification), Duration);
    }
}
