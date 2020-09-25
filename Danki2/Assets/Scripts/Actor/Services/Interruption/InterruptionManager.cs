﻿using System;
using System.Collections.Generic;

public class InterruptionManager
{
    private readonly Dictionary<InterruptionType, List<Action>> interruptionRegister = new EnumDictionary<InterruptionType, List<Action>>(
        () => new List<Action>()
    );

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
        switch (interruptionType)
        {
            case InterruptionType.Soft:
                interruptionRegister[InterruptionType.Soft].ForEach(a => a());
                break;
            case InterruptionType.Hard:
                interruptionRegister[InterruptionType.Soft].ForEach(a => a());
                interruptionRegister[InterruptionType.Hard].ForEach(a => a());
                break;
        }
    }
}
