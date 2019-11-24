using System;
using UnityEngine;

public class Whirlwind : Channel
{
    public Whirlwind(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 3f;

    public override void Cancel()
    {
        GameObject.Find("Player").GetComponent<Renderer>().material.color = Color.cyan;
    }

    public override void Continue()
    {
        var rnd = (float) new System.Random().NextDouble();
        
        GameObject.Find("Player").GetComponent<Renderer>().material.color = new Color(rnd, 1f, 1f);
    }

    public override void End()
    {
        GameObject.Find("Player").GetComponent<Renderer>().material.color = Color.green;
    }

    public override void Start()
    {
        GameObject.Find("Player").GetComponent<Renderer>().material.color = Color.yellow;
    }
}
