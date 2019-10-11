using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortal : Actor
{
    public new void Update()
    {
        // if (this.CurrentHealth <= 0)
        // {
        //     this.Die();
        //     return;
        // }

        base.Update();
    }
}
