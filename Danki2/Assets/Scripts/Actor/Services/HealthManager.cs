using System;
using UnityEngine;

public class HealthManager
{
    private readonly Actor actor;

    public int Health { get; private set; }

    public int MaxHealth => actor.GetStat(Stat.MaxHealth);

    public bool IsDamaged => Health < MaxHealth;

    public Subject<int> DamageSubject { get; } = new Subject<int>();

    private const int MinimumDamageAfterStats = 1;

    public HealthManager(Actor actor, Subject updateSubject)
    {
        this.actor = actor;

        Health = MaxHealth;

        updateSubject.Subscribe(() =>
        {
            Health = Math.Min(Health, MaxHealth);
        });
    }

    public void TickDamage(int damage)
    {
        damage = actor.EffectManager.ProcessIncomingDamage(damage);

        if (damage < 0)
        {
            Debug.LogWarning($"Tried to tick negative damage, value: {damage}");
            return;
        }

        ModifyHealth(-damage);
    }

    public void ReceiveDamage(int damage)
    {
        damage = Mathf.Max(MinimumDamageAfterStats, damage - actor.GetStat(Stat.Defence));
        damage = actor.EffectManager.ProcessIncomingDamage(damage);

        if (damage < 0)
        {
            Debug.LogWarning($"Tried to receive negative damage, value: {damage}");
            return;
        }

        ModifyHealth(-damage);

        actor.InterruptionManager.Interrupt(InterruptionType.Soft);
    }

    public void ReceiveHealing(int healing)
    {
        if (healing < 0) return;

        ModifyHealth(healing);
    }

    private void ModifyHealth(int healthChange)
    {
        if (healthChange < 0)
        {
            DamageSubject.Next(-healthChange);
        }

        Health = Mathf.Clamp(Health + healthChange, 0, MaxHealth);
    }
}