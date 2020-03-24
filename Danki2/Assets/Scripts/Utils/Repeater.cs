using System;
using UnityEngine;

/// <summary>
/// Helper class for running repeated code when outside of a Unity context and coroutines
/// aren't an option. The Update() method should be called every frame once the Repeater
/// has been created.
/// </summary>
public class Repeater
{
    private readonly float interval;
    private readonly Action action;

    private float currentTime = 0;

    /// <param name="interval">How often to run the action, given in seconds</param>
    /// <param name="action">The action to run</param>
    /// <param name="runOnStart">
    /// Whether to run the action when the repeater is created. If false then the action
    /// won't run until the fist time the interval passes.
    /// </param>
    public Repeater(float interval, Action action, bool runOnStart = true)
    {
        this.interval = interval;
        this.action = action;

        if (runOnStart) action.Invoke();
    }

    public void Update()
    {
        currentTime += Time.deltaTime;

        // While loop so that if the deltaTime is greater than the interval, the action
        // still gets called the right number of times.
        while (currentTime >= interval)
        {
            action.Invoke();
            currentTime -= interval;
        }
    }
}
