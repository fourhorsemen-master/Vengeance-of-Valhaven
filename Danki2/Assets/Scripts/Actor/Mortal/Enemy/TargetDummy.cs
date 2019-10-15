using Assets.Scripts.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetDummy : Enemy
{
    private AI<TargetDummy> ai;

    public TargetDummy()
    {
        this.ai = new AI<TargetDummy>(
            this,
            a => new Dictionary<AIAction, bool>(),
            new Dictionary<AIAction, Action<TargetDummy>>()
        );
    }

    public override IAI AI => this.ai;
}
