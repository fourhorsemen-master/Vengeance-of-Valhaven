using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityService2
{
    private const float AbilityRange = 3;

    private const float ImpactDamageIncrease = 1f;
    private const float ExecuteHealthProportion = 0.3f;
    private const float ExecuteDamageMultiplier = 1.5f;
    private const float MaimHealthProportion = 0.7f;
    private const float MaimDamageMultiplier = 1.5f;

    public CastContext CurrentCast { get; private set; }

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
        SerializableGuid abilityId = player.AbilityTree.GetAbilityId(direction);
        AbilityType2 abilityType = AbilityLookup2.Instance.GetAbilityType(abilityId);

        player.AbilityTree.Walk(direction);

        List<Empowerment> empowerments = GetCurrentEmpowerments(selfEmpoweringAbilities);
        List<Empowerment> nextEmpowerments = GetCurrentEmpowerments();

        player.MovementManager.LookAt(targetPosition);
        
        Vector3 castDirection = targetPosition - player.transform.position;
        Quaternion currentCastRotation = AbilityUtils.GetMeleeCastRotation(castDirection);

        Vector3 collisionTemplateOrigin = abilityType == AbilityType2.Smash
            ? player.transform.position + player.transform.forward * 1.2f
            : player.CollisionTemplateSource;

        abilityAnimator.HandleAnimation(abilityId);

        CurrentCast = new CastContext(
            abilityId,
            abilityType,
            currentCastRotation,
            collisionTemplateOrigin,
            empowerments,
            nextEmpowerments
        );
    }

    private void OnImpact()
    {
        bool hasDealtDamage = false;

        AbilityUtils.TemplateCollision(
            player,
            collisionTemplateLookup[AbilityLookup2.Instance.GetAbilityType(CurrentCast.AbilityId)],
            AbilityRange,
            CurrentCast.CollisionTemplateOrigin,
            CurrentCast.CastRotation,
            AbilityTypeLookup.Instance.GetCollisionSoundLevel(AbilityLookup2.Instance.GetAbilityType(CurrentCast.AbilityId)),
            enemyCallback: enemy =>
            {
                hasDealtDamage = true;
                HandleCollision(CurrentCast.AbilityId, enemy, CurrentCast.Empowerments);
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
        damage = HandleImpactDamage(damage, empowerments);
        damage = HandleExecuteDamage(damage, empowerments, enemy);
        damage = HandleMaimDamage(damage, empowerments, enemy);
        return Mathf.CeilToInt(damage);
    }

    private int CalculateBleedStacks(List<Empowerment> empowerments)
    {
        int bleedStacks = empowerments.Count(e => e == Empowerment.Rupture);
        return bleedStacks;
    }

    private float HandleImpactDamage(float damage, List<Empowerment> empowerments)
    {
        return damage + empowerments.Count(e => e == Empowerment.Impact) * ImpactDamageIncrease;
    }

    private float HandleExecuteDamage(float damage, List<Empowerment> empowerments, Enemy enemy)
    {
        if (enemy.HealthManager.HealthProportion <= ExecuteHealthProportion)
        {
            damage *= 1 + (empowerments.Count(e => e == Empowerment.Execute) * (ExecuteDamageMultiplier - 1));
        }
        return damage;
    }

    private float HandleMaimDamage(float damage, List<Empowerment> empowerments, Enemy enemy)
    {
        if (enemy.HealthManager.HealthProportion >= MaimHealthProportion)
        {
            damage *= 1 + (empowerments.Count(e => e == Empowerment.Maim) * (MaimDamageMultiplier - 1));
        }
        return damage;
    }
}
