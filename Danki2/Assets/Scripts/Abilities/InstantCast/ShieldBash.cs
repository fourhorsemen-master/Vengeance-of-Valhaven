using System;
using UnityEngine;

public class ShieldBash : InstantCast
{
    public ShieldBash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        GameObject.Find("Player").GetComponent<Renderer>().material.color = Color.magenta;
    }
}
