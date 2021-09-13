using System.Collections.Generic;
using UnityEngine;

public class AbilityService2
{
    private const float AbilityRange = 3;
    private const float AbilityPauseDuration = 0.3f;

    private static readonly Dictionary<AbilityType2, CollisionTemplateShape> collisionTemplateLookup =
        new Dictionary<AbilityType2, CollisionTemplateShape>
        {
            [AbilityType2.Slash] = CollisionTemplateShape.Wedge90,
            [AbilityType2.Smash] = CollisionTemplateShape.Cylinder,
            [AbilityType2.Thrust] = CollisionTemplateShape.Wedge45,
        };
    
    private readonly Player player;
    private readonly AbilityAnimationListener abilityAnimationListener;
    public Subscription impactSubscription;

    public Subject<AbilityCastInformation> AbilityEventSubject { get; } = new Subject<AbilityCastInformation>();

    public AbilityService2(Player player, AbilityAnimationListener abilityAnimationListener)
    {
        this.player = player;
        this.abilityAnimationListener = abilityAnimationListener;
    }

    public void Cast(Direction direction, Vector3 targetPosition)
    {
        SerializableGuid abilityId = player.AbilityTree.GetAbilityId(direction);
        List<Empowerment> empowerments = GetActiveEmpowerments(abilityId);

        player.MovementManager.LookAt(targetPosition);
        player.MovementManager.Pause(AbilityPauseDuration);
        
        Vector3 castDirection = targetPosition - player.transform.position;
        Quaternion castRotation = AbilityUtils.GetMeleeCastRotation(castDirection);

        impactSubscription = abilityAnimationListener.ImpactSubject.Subscribe(() =>
        {
            bool hasDealtDamage = false;

            AbilityUtils.TemplateCollision(
                player,
                collisionTemplateLookup[AbilityLookup2.Instance.GetAbilityType(abilityId)],
                CalculateRange(empowerments),
                player.CollisionTemplateSource,
                castRotation,
                AbilityLookup2.Instance.GetCollisionSoundLevel(abilityId),
                enemyCallback: enemy =>
                {
                    hasDealtDamage = true;
                    HandleCollision(abilityId, enemy, empowerments);
                }
            );

            AbilityEventSubject.Next(new AbilityCastInformation(
                abilityId,
                hasDealtDamage,
                empowerments,
                castRotation,
                CastEvent.Impact
            ));

            impactSubscription.Unsubscribe();
        });      
                
        player.AbilityTree.Walk(direction);
        AbilityEventSubject.Next(new AbilityCastInformation(
            abilityId,
            false,
            empowerments,
            castRotation,
            CastEvent.Cast
        ));
    }

    private List<Empowerment> GetActiveEmpowerments(SerializableGuid abilityId)
    {
        List<Empowerment> empowerments = new List<Empowerment>();

        player.AbilityTree.CurrentNode.IterateUp(
            node => empowerments.AddRange(AbilityLookup2.Instance.GetEmpowerments(node.AbilityId)),
            node => !node.IsRootNode
        );

        empowerments.AddRange(AbilityLookup2.Instance.GetEmpowerments(abilityId));

        return empowerments;
    }

    private float CalculateRange(List<Empowerment> empowerments)
    {
        float range = AbilityRange;
        empowerments.ForEach(e => range *= e == Empowerment.DoubleRange ? 2 : 1);
        return range;
    }

    private void HandleCollision(SerializableGuid abilityId, Enemy enemy, List<Empowerment> empowerments)
    {
        enemy.HealthManager.ReceiveDamage(CalculateDamage(abilityId, empowerments), player);
        enemy.EffectManager.AddStacks(StackingEffect.Bleed, CalculateBleedStacks(empowerments));
    }

    private int CalculateDamage(SerializableGuid abilityId, List<Empowerment> empowerments)
    {
        int damage = AbilityLookup2.Instance.GetDamage(abilityId);
        empowerments.ForEach(e => damage *= e == Empowerment.DoubleDamage ? 2 : 1);
        return damage;
    }

    private int CalculateBleedStacks(List<Empowerment> empowerments)
    {
        int bleedStacks = 0;
        empowerments.ForEach(e => bleedStacks += e == Empowerment.Rupture ? 1 : 0);
        return bleedStacks;
    }
}
