﻿using System;
using UnityEngine;

public class HealthManager
{
    private readonly Actor actor;

    // Note health is clamped below at 0
    public int Health { get; private set; }

    public int MaxHealth => actor.StatsManager.Get(Stat.MaxHealth);

    public float HealthProportion => (float)Health / MaxHealth;

    public Subject<DamageData> UnmodifiedDamageSubject { get; } = new Subject<DamageData>();
    public Subject<DamageData> ModifiedDamageSubject { get; } = new Subject<DamageData>();
    public Subject<int> UnmodifiedTickDamageSubject { get; } = new Subject<int>();
    public Subject<int> ModifiedTickDamageSubject { get; } = new Subject<int>();
    public Subject DamageSubject { get; } = new Subject();
    public Subject<int> HealSubject { get; } = new Subject<int>();

    private const int MinimumDamageAfterStats = 1;

    public HealthManager(Actor actor, Subject updateSubject)
    {
        this.actor = actor;

        Health = PersistenceManager.Instance.SaveData.PlayerHealth;

        updateSubject.Subscribe(() =>
        {
            Health = Math.Min(Health, MaxHealth);
        });

        ModifiedTickDamageSubject.Subscribe(_ => DamageSubject.Next());
        ModifiedDamageSubject.Subscribe(_ => DamageSubject.Next());
    }

    public void TickDamage(int damage)
    {
        if (actor.Dead) return;

        UnmodifiedTickDamageSubject.Next(damage);

        if (damage < 0)
        {
            Debug.LogWarning($"Tried to tick negative damage, value: {damage}");
            return;
        }

        if (damage > 0)
        {
            ModifyHealth(-damage);
            ModifiedTickDamageSubject.Next(damage);
        }
    }

    public void ReceiveDamage(int damage, Actor source)
    {
        if (actor.Dead) return;

        UnmodifiedDamageSubject.Next(new DamageData(damage, source));

        // If already 0, damage should be left as 0, else reduce according to defence, but not below the minimum threshold.
        damage = damage == 0 ? 0 : Mathf.Max(MinimumDamageAfterStats, damage - actor.StatsManager.Get(Stat.Defence));

        if (damage < 0)
        {
            Debug.LogWarning($"Tried to receive negative damage, value: {damage}");
            return;
        }

        if (damage > 0 && !actor.EffectManager.HasPassiveEffect(PassiveEffect.Block))
        {
            ModifyHealth(-damage);
            ModifiedDamageSubject.Next(new DamageData(damage, source));

            actor.InterruptionManager.Interrupt(InterruptionType.Soft);
        }            
    }

    public void ReceiveHeal(int healing)
    {
        if (actor.Dead) return;

        if (healing < 0)
        {
            Debug.LogWarning($"Tried to receive negative heal, value: {healing}");
            return;
        }

        if (healing > 0)
        {
            HealSubject.Next(healing);
            ModifyHealth(healing);
        }
    }

    private void ModifyHealth(int healthChange)
    {
        Health = Mathf.Clamp(Health + healthChange, 0, MaxHealth);
    }
}
