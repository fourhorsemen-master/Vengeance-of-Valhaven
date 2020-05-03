using System;
using UnityEngine;

public class HealthManager
{
    private readonly Actor actor;

    public int Health { get; private set; }

    public int MaxHealth => actor.GetStat(Stat.MaxHealth);

    public bool IsDamaged => Health < MaxHealth;

    public readonly Subject<int> DamageSubject = new Subject<int>();

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
        if (damage < 0)
        {
            Debug.LogWarning($"Tried to tick negative damage, value: {damage}");
            return;
        }

        ModifyHealth(-damage);
    }

    public void ReceiveDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning($"Tried to receive negative damage, value: {damage}");
            return;
        }

        // TODO: Pass this damage through a defensive pipeline.
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