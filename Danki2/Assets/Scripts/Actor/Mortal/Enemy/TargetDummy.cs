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
            (a, b) => new Agenda(),
            new Personality<TargetDummy>()
        );
    }

    public override AI AI => _ai;
}