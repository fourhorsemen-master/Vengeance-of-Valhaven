using System;
using UnityEngine;

public class HealthManager
{
    private readonly Actor actor;

    public int Health { get; private set; }

    public int MaxHealth => actor.GetStat(Stat.MaxHealth);

    public bool IsDamaged => Health < MaxHealth;

    public Subject<int> DamageSubject = new Subject<int>();

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
        if (damage < 0) return;

        DamageSubject.Next(damage);
        Health = Math.Max(Health - damage, 0);
    }

    public void ReceiveDamage(int damage)
    {
        if (damage < 0) return;

        // TODO: Pass this damage through a defensive pipeline.
        DamageSubject.Next(damage);
        Health = Math.Max(Health - damage, 0);

        actor.InterruptionManager.Interrupt(InterruptionType.Soft);
    }

    public void ReceiveHealing(int healing)
    {
        if (healing < 0) return;

        Health = Math.Max(Health + healing, MaxHealth);
    }
}