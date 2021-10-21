using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityService2
{
    private const float AbilityRange = 3;

    public SerializableGuid CurrentAbilityId { get; private set; }
    public Quaternion CurrentCastRotation { get; private set; }
    public List<Empowerment> CurrentEmpowerments { get; private set; }
    public List<Empowerment> NextEmpowerments { get; private set; }

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

    private const float ExecuteHealthProportion = 0.3f;
    private const float ExecuteDamageMultiplier = 1.5f;

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
        player.AbilityTree.Walk(direction);

        CurrentEmpowerments = GetCurrentEmpowerments(selfEmpoweringAbilities);
        NextEmpowerments = GetCurrentEmpowerments();

        player.MovementManager.LookAt(targetPosition);
        
        Vector3 castDirection = targetPosition - player.transform.position;
        CurrentCastRotation = AbilityUtils.GetMeleeCastRotation(castDirection);

        abilityAnimator.HandleAnimation(CurrentAbilityId);
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

    private List<Empowerment> GetCurrentEmpowerments(bool includeCurrentAbility = true)
    {
        List<Empowerment> empowerments = new List<Empowerment>();

        Node iterateFromNode = includeCurrentAbility
            ? player.AbilityTree.CurrentNode
            : player.AbilityTree.CurrentNode.Parent;

        iterateFromNode.IterateUp(
            node => empowerments.AddRange(AbilityLookup2.Instance.GetEmpowerments(node.AbilityId)),
            node => !node.IsRootNode
        );

        // Reversing here ensures empowerments are listed in chronological order
        empowerments.Reverse();

        return empowerments;
    }

    private void HandleCollision(SerializableGuid abilityId, Enemy enemy, List<Empowerment> empowerments)
    {
        enemy.HealthManager.ReceiveDamage(CalculateDamage(abilityId, empowerments, enemy), player);
        enemy.EffectManager.AddStacks(StackingEffect.Bleed, CalculateBleedStacks(empowerments));
    }

    private int CalculateDamage(SerializableGuid abilityId, List<Empowerment> empowerments, Enemy enemy)
    {
        float damage = AbilityLookup2.Instance.GetDamage(abilityId);
        damage += empowerments.Count(e => e == Empowerment.Impact);
        if(enemy.HealthManager.HealthProportion <= ExecuteHealthProportion)
        {
            damage *= 1 + (empowerments.Count(e => e == Empowerment.Execute) * (ExecuteDamageMultiplier - 1));
        }
        return Mathf.CeilToInt(damage);
    }

    private int CalculateBleedStacks(List<Empowerment> empowerments)
    {
        int bleedStacks = empowerments.Count(e => e == Empowerment.Rupture);
        return bleedStacks;
    }
}
