﻿using System;
using System.Collections.Generic;

public class InterruptionManager
{
    private readonly Dictionary<InterruptionType, List<Action>> interruptionRegister = new EnumDictionary<InterruptionType, List<Action>>(
        () => new List<Action>()
    );

    private readonly Actor actor;

    public InterruptionManager(Actor actor, Subject startSubject)
    {
        this.actor = actor;
        actor.DeathSubject.Subscribe(HardInterrupt);
        startSubject.Subscribe(() => Setup(actor.MovementManager));
    }

    public void Setup(MovementManager movementManager)
    {
        movementManager.MoveLockSubject.Subscribe(() => Interrupt(InterruptionType.Hard));
    }

    public void Register(InterruptionType interruptionType, Action action)
    {
        interruptionRegister[interruptionType].Add(action);
    }

    public void Interrupt(InterruptionType interruptionType)
    {
        if (actor.Dead) return;

        switch (interruptionType)
        {
            case InterruptionType.Soft:
                SoftInterrupt();
                break;
            case InterruptionType.Hard:
                HardInterrupt();
                break;
        }
    }

    private void SoftInterrupt()
    {
        interruptionRegister[InterruptionType.Soft].ForEach(a => a());
    }

    private void HardInterrupt()
    {
        interruptionRegister[InterruptionType.Soft].ForEach(a => a());
        interruptionRegister[InterruptionType.Hard].ForEach(a => a());
    }
}
