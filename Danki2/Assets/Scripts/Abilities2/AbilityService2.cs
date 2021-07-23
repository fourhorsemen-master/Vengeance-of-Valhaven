using System.Collections.Generic;
using UnityEngine;

public class AbilityService2
{
    private const float AbilityRange = 3;
    private const float AbilityPauseDuration = 0.3f;
    
    private readonly Player player;
    
    public Subject AbilityCastSubject { get; } = new Subject();

    public AbilityService2(Player player)
    {
        this.player = player;
    }

    public void Cast(Direction direction, Vector3 targetPosition)
    {
        Ability2 ability = player.AbilityTree.GetAbility(direction);
        List<Empowerment> empowerments = GetActiveEmpowerments(ability);

        player.MovementManager.LookAt(targetPosition);
        player.MovementManager.Pause(AbilityPauseDuration);
        
        Vector3 castDirection = targetPosition - player.transform.position;
        Quaternion castRotation = AbilityUtils.GetMeleeCastRotation(castDirection);
        
        bool hasDealtDamage = false;
        
        AbilityUtils.TemplateCollision(
            player,
            CollisionTemplateShape.Wedge90,
            CalculateRange(empowerments),
            player.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                hasDealtDamage = true;
                HandleCollision(ability, actor, empowerments);
            },
            AbilityLookup2.Instance.GetCollisionSoundLevel(ability)
        );
        
        HandleCameraShake(hasDealtDamage);
        
        player.AbilityTree.Walk(direction);
        AbilityCastSubject.Next();
    }

    private List<Empowerment> GetActiveEmpowerments(Ability2 ability)
    {
        List<Empowerment> empowerments = new List<Empowerment>();

        player.AbilityTree.CurrentNode.IterateUp(
            node => empowerments.AddRange(AbilityLookup2.Instance.GetEmpowerments(node.Ability)),
            node => !node.IsRootNode
        );

        empowerments.AddRange(AbilityLookup2.Instance.GetEmpowerments(ability));

        return empowerments;
    }

    private float CalculateRange(List<Empowerment> empowerments)
    {
        float range = AbilityRange;
        empowerments.ForEach(e => range *= e == Empowerment.DoubleRange ? 2 : 1);
        return range;
    }

    private void HandleCollision(Ability2 ability, Actor actor, List<Empowerment> empowerments)
    {
        actor.HealthManager.ReceiveDamage(CalculateDamage(ability, empowerments), player);
        actor.EffectManager.AddStacks(StackingEffect.Bleed, CalculateBleedStacks(empowerments));
    }

    private int CalculateDamage(Ability2 ability, List<Empowerment> empowerments)
    {
        int damage = AbilityLookup2.Instance.GetDamage(ability);
        empowerments.ForEach(e => damage *= e == Empowerment.DoubleDamage ? 2 : 1);
        return damage;
    }

    private int CalculateBleedStacks(List<Empowerment> empowerments)
    {
        int bleedStacks = 0;
        empowerments.ForEach(e => bleedStacks += e == Empowerment.Rupture ? 1 : 0);
        return bleedStacks;
    }

    private void HandleCameraShake(bool hasDealtDamage)
    {
        CustomCamera.Instance.AddShake(hasDealtDamage ? ShakeIntensity.Medium : ShakeIntensity.Low);
    }
}
