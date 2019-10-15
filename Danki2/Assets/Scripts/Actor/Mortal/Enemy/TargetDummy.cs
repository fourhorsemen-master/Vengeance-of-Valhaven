using Assets.Scripts.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetDummy : Enemy
{
    private AI ai;

    public TargetDummy()
    {
        this.ai = new AI(
            this,
            a => new Dictionary<AIAction, bool>(),
            new Dictionary<AIAction, Action<Actor>>()
        );
    }

    public override AI AI => this.ai;
}
