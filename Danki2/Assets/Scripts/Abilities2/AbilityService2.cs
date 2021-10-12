using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityService2
{
    private const float AbilityRange = 3;

    public SerializableGuid CurrentAbilityId { get; private set; }
    public Quaternion CurrentCastRotation { get; private set; }
    public List<Empowerment> CurrentEmpowerments { get; private set; }

    private static readonly Dictionary<AbilityType2, CollisionTemplateShape> collisionTemplateLookup =
        new Dictionary<AbilityType2, CollisionTemplateShape>
        {
            [AbilityType2.Slash] = CollisionTemplateShape.Wedge90,
            [AbilityType2.Smash] = CollisionTemplateShape.Cylinder,
            [AbilityType2.Thrust] = CollisionTemplateShape.Wedge45,
        };
    
    private readonly Player player;
    private readonly AbilityAnimator abilityAnimator;

    public AbilityService2(Player player, AbilityAnimator abilityAnimator)
    {
        this.player = player;
        this.abilityAnimator = abilityAnimator;

        player.AbilityAnimationListener.ImpactSubject.Subscribe(OnImpact);
    }

    public void Cast(Direction direction, Vector3 targetPosition)
    {
        CurrentAbilityId = player.AbilityTree.GetAbilityId(direction);
        CurrentEmpowerments = GetActiveEmpowerments(CurrentAbilityId);

        player.MovementManager.LookAt(targetPosition);
        
        Vector3 castDirection = targetPosition - player.transform.position;
        CurrentCastRotation = AbilityUtils.GetMeleeCastRotation(castDirection);

        abilityAnimator.HandleAnimation(CurrentAbilityId);

        player.AbilityTree.Walk(direction);
    }

    private void OnImpact()
    {
        bool hasDealtDamage = false;

        AbilityUtils.TemplateCollision(
            player,
            collisionTemplateLookup[AbilityLookup2.Instance.GetAbilityType(CurrentAbilityId)],
            AbilityRange,
            player.CollisionTemplateSource,
            CurrentCastRotation,
            AbilityTypeLookup.Instance.GetCollisionSoundLevel(AbilityLookup2.Instance.GetAbilityType(CurrentAbilityId)),
            enemyCallback: enemy =>
            {
                hasDealtDamage = true;
                HandleCollision(CurrentAbilityId, enemy, CurrentEmpowerments);
            }
        );

        ShakeIntensity shakeIntensity = hasDealtDamage
            ? ShakeIntensity.High
            : ShakeIntensity.Low;
        CustomCamera.Instance.AddShake(shakeIntensity);
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

    private void HandleCollision(SerializableGuid abilityId, Enemy enemy, List<Empowerment> empowerments)
    {
        enemy.HealthManager.ReceiveDamage(CalculateDamage(abilityId, empowerments), player);
        enemy.EffectManager.AddStacks(StackingEffect.Bleed, CalculateBleedStacks(empowerments));
    }

    private int CalculateDamage(SerializableGuid abilityId, List<Empowerment> empowerments)
    {
        int damage = AbilityLookup2.Instance.GetDamage(abilityId);
        damage += empowerments.Count(e => e == Empowerment.Impact);
        return damage;
    }

    private int CalculateBleedStacks(List<Empowerment> empowerments)
    {
        int bleedStacks = empowerments.Count(e => e == Empowerment.Rupture);
        return bleedStacks;
    }
}
