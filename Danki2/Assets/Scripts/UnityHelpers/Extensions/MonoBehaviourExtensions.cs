using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static Coroutine NextFrame(this MonoBehaviour monoBehaviour, Action action)
    {
        return monoBehaviour.StartCoroutine(DelayedAction(0, action));
    }

    public static Coroutine WaitAndAct(this MonoBehaviour monoBehaviour, float waitTime, Action action)
    {
        return monoBehaviour.StartCoroutine(DelayedAction(waitTime, action));
    }

    public static Coroutine ActOnInterval(this MonoBehaviour monoBehaviour, float interval, Action<int> action, float startDelay = 0f, int? numRepetitions = null)
    {
        return ActOnRandomisedInterval(monoBehaviour, interval, interval, action, startDelay, numRepetitions);
    }

    public static Coroutine ActOnRandomisedInterval(this MonoBehaviour monoBehaviour, float minInterval, float maxInterval, Action<int> action, float startDelay = 0f, int? numRepetitions = null)
    {
        if (numRepetitions == null)
        {
            return monoBehaviour.StartCoroutine(IntervalAction(minInterval, maxInterval, action, startDelay));
        }
        else
        {
            return monoBehaviour.StartCoroutine(IntervalAction(minInterval, maxInterval, action, startDelay, numRepetitions.Value));
        }
    }

    public static Coroutine WaitForFixedUpdateAndAct(this MonoBehaviour monoBehaviour, Action action)
    {
        return monoBehaviour.StartCoroutine(PostFixedUpdateAction(action));
    }

    private static IEnumerator DelayedAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action();
    }

    /// <summary>
    /// Invokes an action on an interval.
    /// </summary>
    /// <param name="minInterval">Minimum time between each invocation.</param>
    /// <param name="maxInterval">Maximum time between each invocation.</param>
    /// <param name="action">The action to perform. Takes it's 0-based index as a parameter.</param>
    /// <param name="startDelay">Delay before first invocation.</param>
    /// <param name="numRepetitions">Optional. Limits the number of repetitions.</param>
    /// <returns></returns>
    private static IEnumerator IntervalAction(float minInterval, float maxInterval, Action<int> action, float startDelay, int? numRepetitions = null)
    {
        yield return new WaitForSeconds(startDelay);

        int counter = 0;

        while(true)
        {
            action(counter);
            float interval = UnityEngine.Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            counter ++;

            if (numRepetitions.HasValue && counter == numRepetitions) break;
        }
    }

    private static IEnumerator PostFixedUpdateAction(Action action)
    {
        yield return new WaitForFixedUpdate();

        action();
    }
}
