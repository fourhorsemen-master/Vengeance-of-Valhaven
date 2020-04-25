using System;
using UnityEngine;

public class HealthManager
{
    private readonly Actor actor;

    public int Health { get; private set; }

    public int MaxHealth { get; private set; }

    public bool IsDamaged => Health < MaxHealth;

    public HealthManager(Actor actor, Subject updateSubject)
    {
        this.actor = actor;

        MaxHealth = actor.GetStat(Stat.MaxHealth);
        Health = MaxHealth;

        updateSubject.Subscribe(() =>
        {
            MaxHealth = actor.GetStat(Stat.MaxHealth);
            if (Health > MaxHealth) Health = MaxHealth;
        });
    }

    public void TickDamage(int damage)
    {
        Health = Math.Max(Health - damage, 0);
    }

    public void DealDamage(int damage)
    {
        // I think we should expect that the damage amount coming out of an offensive pipeline might be negative, and just ignore it if so.
        if (damage < 0) return;

        // TODO: Pass this damage through a defensive pipeline.
        Health = Math.Max(Health - damage, 0);

        actor.InterruptionManager.Interrupt(InterruptionType.Soft);
    }

    public void Heal(int healing)
    {
        // Like damage, ignore negative values.
        if (healing < 0) return;

        Health = Math.Max(Health + healing, MaxHealth);
    }
}