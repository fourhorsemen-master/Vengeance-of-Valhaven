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

    public static Coroutine ActOnInterval(this MonoBehaviour monoBehaviour, float interval, Action<int> action, float? initialInterval = null, int? numRepetitions = null)
    {
        return monoBehaviour.StartCoroutine(IntervalAction(() => interval, action, initialInterval, numRepetitions));
    }

    public static Coroutine ActOnRandomisedInterval(this MonoBehaviour monoBehaviour, float minInterval, float maxInterval, Action<int> action, float? initialInterval = null, int? numRepetitions = null)
    {
        return monoBehaviour.StartCoroutine(IntervalAction(() => UnityEngine.Random.Range(minInterval, maxInterval), action, initialInterval, numRepetitions));
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
    /// <param name="initialInterval">Delay before first invocation.</param>
    /// <param name="numRepetitions">Optional. Limits the number of repetitions.</param>
    /// <returns></returns>
    private static IEnumerator IntervalAction(Func<float> intervalCalculator, Action<int> action, float? initialInterval, int? numRepetitions)
    {
        yield return new WaitForSeconds(initialInterval ?? intervalCalculator());

        int counter = 0;

        while(true)
        {
            action(counter);
            float interval = intervalCalculator();
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
