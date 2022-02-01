using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityService
{
    private const float AbilityRange = 3;

    private const float ImpactDamageIncrease = 1f;
    private const float DuelDamageIncrease = 2f;
    private const float BrawlDamageIncrease = 2f;
    private const float ExecuteHealthProportion = 0.3f;
    private const float ExecuteDamageMultiplier = 1.5f;
    private const float MaimHealthProportion = 0.7f;
    private const float MaimDamageMultiplier = 1.5f;
    private const int ShockMaxJumps = 2;
    private const int ShockDamage = 3;
    private const float ShockJumpRange = 8f;
    private const float EnvenomPoisonDuration = 10f;

    public CastContext CurrentCast { get; private set; }

    private static readonly Dictionary<AbilityType, CollisionTemplateShape> collisionTemplateLookup =
        new Dictionary<AbilityType, CollisionTemplateShape>
        {
            [AbilityType.Slash] = CollisionTemplateShape.Wedge90,
            [AbilityType.Smash] = CollisionTemplateShape.Cylinder,
            [AbilityType.Lunge] = CollisionTemplateShape.Wedge45,
        };
    
    private readonly Player player;
    private readonly AbilityAnimator abilityAnimator;
    private readonly bool selfEmpoweringAbilities;

    public AbilityService(Player player, AbilityAnimator abilityAnimator, bool selfEmpoweringAbilities)
    {
        this.player = player;
        this.abilityAnimator = abilityAnimator;
        this.selfEmpoweringAbilities = selfEmpoweringAbilities;
        player.AbilityAnimationListener.ImpactSubject.Subscribe(OnImpact);
    }

    public void Cast(Direction direction, Vector3 targetPosition)
    {
        Ability ability = player.AbilityTree.GetAbility(direction);

        player.AbilityTree.Walk(direction);

        List<Empowerment> empowerments = GetCurrentEmpowerments(selfEmpoweringAbilities);
        List<Empowerment> nextEmpowerments = GetCurrentEmpowerments();

        player.MovementManager.LookAt(targetPosition);
        
        Vector3 castDirection = targetPosition - player.transform.position;
        Quaternion currentCastRotation = AbilityUtils.GetMeleeCastRotation(castDirection);

        Vector3 collisionTemplateOrigin = ability.Type == AbilityType.Smash
            ? player.transform.position + player.transform.forward * 1.2f
            : player.CollisionTemplateSource;

        abilityAnimator.HandleAnimation(ability.Type);

        CurrentCast = new CastContext(
            ability,
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
            collisionTemplateLookup[CurrentCast.Ability.Type],
            AbilityRange,
            CurrentCast.CollisionTemplateOrigin,
            CurrentCast.CastRotation,
            AbilityTypeLookup.Instance.GetCollisionSoundLevel(CurrentCast.Ability.Type),
            enemyCallback: enemy =>
            {
                hasDealtDamage = true;
                HandleCollision(CurrentCast.Ability, enemy, CurrentCast.Empowerments);
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
            node => empowerments.AddRange(node.Ability.Empowerments),
            node => !node.IsRootNode
        );

        // Reversing here ensures empowerments are listed in chronological order
        empowerments.Reverse();

        return empowerments;
    }

    private void HandleCollision(Ability ability, Enemy enemy, List<Empowerment> empowerments)
    {
        enemy.EffectManager.AddStacks(StackingEffect.Bleed, CalculateBleedStacks(empowerments));
        enemy.EffectManager.AddStacks(StackingEffect.Purge, CalculatePurgeStacks(empowerments));
        if (empowerments.Contains(Empowerment.Envenom))
        {
            enemy.EffectManager.AddActiveEffect(ActiveEffect.Poison, EnvenomPoisonDuration);
        }
        enemy.HealthManager.ReceiveDamage(CalculateDamage(ability, empowerments, enemy), player, empowerments);
        HandleShock(enemy, empowerments);
    }

    private int CalculateDamage(Ability ability, List<Empowerment> empowerments, Enemy enemy)
    {
        float damage = ability.Damage;
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

    private int CalculatePurgeStacks(List<Empowerment> empowerments)
    {
        int purgeStacks = empowerments.Count(e => e == Empowerment.Exorcise);
        return purgeStacks;
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

    private void HandleShock(Enemy enemy, List<Empowerment> empowerments)
    {
        if (!empowerments.Contains(Empowerment.Shock)) return;

        int lightningDamage = empowerments.Count(x => x == Empowerment.Shock) * ShockDamage;

        ChainLightning.Fire(enemy, lightningDamage, ShockMaxJumps, ShockJumpRange);
    }
}
