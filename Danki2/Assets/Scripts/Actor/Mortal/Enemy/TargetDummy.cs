using Assets.Scripts.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetDummy : Enemy
{
    private AI<TargetDummy> _ai;

    public TargetDummy()
    {
        _ai = new AI<TargetDummy>(
            this,
            a => new Dictionary<AIAction, bool>(),
            new Dictionary<AIAction, Action<TargetDummy>>()
        );
    }

    public override IAI AI => _ai;
}
