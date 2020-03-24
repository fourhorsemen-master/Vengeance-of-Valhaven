using System;
using UnityEngine;

public class Repeater
{
    private readonly float interval;
    private readonly Action action;

    private float currentTime = 0;

    public Repeater(float interval, Action action, bool runOnStart = true)
    {
        this.interval = interval;
        this.action = action;

        if (runOnStart) action.Invoke();
    }

    public void Update()
    {
        currentTime += Time.deltaTime;

        while (currentTime >= interval)
        {
            action.Invoke();
            currentTime -= interval;
        }
    }
}
