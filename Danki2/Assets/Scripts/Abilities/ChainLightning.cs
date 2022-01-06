using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainLightning
{
    private readonly int damage;
    private readonly int maxJumps;
    private readonly float jumpRange;

    private readonly List<Actor> affectedTargets = new List<Actor>();

    private ChainLightning(int damage, int maxJumps, float jumpRange)
    {
        this.damage = damage;
        this.maxJumps = maxJumps;
        this.jumpRange = jumpRange;
    }

    public static void Fire(Enemy enemy, int damage, int maxJumps, float maxRange)
    {
        new ChainLightning(damage, maxJumps, maxRange).Fire(enemy);
    }

    private void Fire(Enemy enemy)
    {
        Damage(enemy);

        while (affectedTargets.Count < maxJumps + 1)
        {
            List<Enemy> possibleTargets = ActorCache.Instance.Cache.Select(x => x.Actor)
                .Where(x => x.CompareTag(Tag.Enemy))
                .Except(affectedTargets)
                .Where(x => Vector3.Distance(x.Centre, affectedTargets.Last().Centre) < jumpRange)
                .Select(x => (Enemy)x)
                .ToList();

            if (!possibleTargets.Any()) break;

            Enemy nextTarget = RandomUtils.Choice(possibleTargets);

            LightningChainVisual.Create(affectedTargets.Last().Centre, nextTarget.Centre);

            Damage(nextTarget);
        }
    }

    private void Damage(Enemy enemy)
    {
        LightningImpactVisual.Create(enemy.Centre);
        enemy.HealthManager.ReceiveDamage(damage, ActorCache.Instance.Player);
        affectedTargets.Add(enemy);
    }
}
