using System;
using UnityEngine;

public class HealthManager
{
    private readonly Actor actor;

    public int Health { get; private set; }

    public int MaxHealth => actor.GetStat(Stat.MaxHealth);

    public bool IsDamaged => Health < MaxHealth;

    public Subject<int> DamageSubject { get; } = new Subject<int>();
    public Subject<Tuple<int, Actor>> DamageSourceSubject { get; } = new Subject<Tuple<int, Actor>>();
    public Subject<int> TickDamageSubject { get; } = new Subject<int>();
    public Subject<int> HealSubject { get; } = new Subject<int>();

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
        TickDamageSubject.Next(damage);
    }

    public void ReceiveDamage(int damage, Actor source)
    {
        // If already 0, damage should be left as 0, else reduce according to defence, but not below the minimum threshold.
        damage = damage == 0 ? 0 : Mathf.Max(MinimumDamageAfterStats, damage - actor.GetStat(Stat.Defence));
        damage = actor.EffectManager.ProcessIncomingDamage(damage);

        if (damage < 0)
        {
            Debug.LogWarning($"Tried to receive negative damage, value: {damage}");
            return;
        }

        ModifyHealth(-damage);
        DamageSubject.Next(damage);
        DamageSourceSubject.Next(Tuple.Create(damage, source));

        actor.InterruptionManager.Interrupt(InterruptionType.Soft);
    }

    public void ReceiveHeal(int healing)
    {
        healing = actor.EffectManager.ProcessIncomingHeal(healing);

        if (healing < 0)
        {
            Debug.LogWarning($"Tried to receive negative heal, value: {healing}");
            return;
        }

        HealSubject.Next(healing);
        ModifyHealth(healing);
    }

    private void ModifyHealth(int healthChange)
    {
        Health = Mathf.Clamp(Health + healthChange, 0, MaxHealth);
    }
}