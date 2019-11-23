using System;
using UnityEngine;

public class Slash : InstantCast
{
    public Slash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        GameObject.Find("Player").GetComponent<Renderer>().material.color = Color.blue;
    }
}
