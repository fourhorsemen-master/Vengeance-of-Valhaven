using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void WaitAndAct(this MonoBehaviour monoBehaviour, float waitTime, Action action)
    {
        monoBehaviour.StartCoroutine(DelayedAction(waitTime, action));
    }

    private static IEnumerator DelayedAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action.Invoke();
    }
}
