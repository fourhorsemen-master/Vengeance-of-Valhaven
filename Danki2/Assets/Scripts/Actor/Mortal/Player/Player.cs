using Assets.Scripts.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mortal
{
    public override IAI AI => null;

    protected override void Act()
    {
        // TODO: Act according to key-presses.
    }
}
