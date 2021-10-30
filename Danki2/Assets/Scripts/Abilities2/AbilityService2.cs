using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityService2
{
    private const float AbilityRange = 3;

    private const float ImpactDamageIncrease = 1f;
    private const float DuelDamageIncrease = 2f;
    private const float BrawlDamageIncrease = 2f;
    private const float ExecuteHealthProportion = 0.3f;
    private const float ExecuteDamageMultiplier = 1.5f;
    private const float MaimHealthProportion = 0.7f;
    private const float MaimDamageMultiplier = 1.5f;
    private const float SiphonCurrencyMultipier = 1.5f;

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
        enemy.EffectManager.AddStacks(StackingEffect.Bleed, CalculateBleedStacks(empowerments));
        enemy.HealthManager.ReceiveDamage(CalculateDamage(abilityId, empowerments, enemy), player);

        if (enemy.HealthManager.Health <= 0)
        {
            int siphonCount = empowerments.Count(e => e == Empowerment.Siphon);
            enemy.AddCurrencyValueMultiplier(1 + siphonCount * (SiphonCurrencyMultipier - 1));
        }
    }

    private int CalculateDamage(SerializableGuid abilityId, List<Empowerment> empowerments, Enemy enemy)
    {
        float damage = AbilityLookup2.Instance.GetDamage(abilityId);
        damage = HandleImpactDamage(damage, empowerments);
        damage = HandleDuelDamage(damage, empowerments);
        damage = HandleBrawlDamage(damage, empowerments);
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

    private float HandleDuelDamage(float damage, List<Empowerment> empowerments)
    {
        return CombatRoomManager.Instance.TotalEnemyCount - CombatRoomManager.Instance.DeadEnemyCount == 1
            ? damage + empowerments.Count(e => e == Empowerment.Duel) * DuelDamageIncrease
            : damage;
    }

    private float HandleBrawlDamage(float damage, List<Empowerment> empowerments)
    {
        return CombatRoomManager.Instance.TotalEnemyCount - CombatRoomManager.Instance.DeadEnemyCount > 1
            ? damage + empowerments.Count(e => e == Empowerment.Brawl) * BrawlDamageIncrease
            : damage;
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
