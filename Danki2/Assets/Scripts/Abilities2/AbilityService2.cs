using System.Collections.Generic;
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
    private readonly bool selfEmpoweringAbilities;

    public AbilityService2(Player player, AbilityAnimator abilityAnimator, bool selfEmpoweringAbilities)
    {
        this.player = player;
        this.abilityAnimator = abilityAnimator;
        this.selfEmpoweringAbilities = selfEmpoweringAbilities;
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
            CalculateRange(CurrentEmpowerments),
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

        if (selfEmpoweringAbilities)
        {
            empowerments.AddRange(AbilityLookup2.Instance.GetEmpowerments(abilityId));
        }

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
