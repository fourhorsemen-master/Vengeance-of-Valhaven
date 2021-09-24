using System;
using UnityEngine;

public class ForestGolemBoulder : MonoBehaviour
{
    [SerializeField] private LifetimeTracker lifetimeTracker = null;
    [SerializeField] private AnimationCurve trajectoryCurve = null;
    [SerializeField] private float smashScaleFactor = 0;

    private float animationEndTime;
    private float spikeDamageInitialDelay;
    private float spikeDamageInterval;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Action callback;
    private Action spikeDamageCallback;

    private bool finished = false;

    public static void Create(
        ForestGolemBoulder prefab,
        Vector3 startPosition,
        Vector3 endPosition,
        Action callback,
        Action spikeDamageCallback,
        float spikeDamageInitialDelay,
        float spikeDamageInterval
    )
    {
        ForestGolemBoulder forestGolemBoulder = Instantiate(prefab, startPosition, Quaternion.identity);
        forestGolemBoulder.startPosition = startPosition;
        forestGolemBoulder.endPosition = endPosition;
        forestGolemBoulder.callback = callback;
        forestGolemBoulder.spikeDamageCallback = spikeDamageCallback;
        forestGolemBoulder.spikeDamageInitialDelay = spikeDamageInitialDelay;
        forestGolemBoulder.spikeDamageInterval = spikeDamageInterval;
    }

    private void Start()
    {
        animationEndTime = AnimationCurveUtils.GetEndTime(trajectoryCurve);
    }

    private void Update()
    {
        if (finished) return;
        
        if (lifetimeTracker.Lifetime > animationEndTime)
        {
            transform.position = endPosition;
            SmashObject.Create(endPosition, smashScaleFactor);
            callback();
            finished = true;
            this.ActOnInterval(spikeDamageInterval, _ => spikeDamageCallback(), spikeDamageInitialDelay);
            return;
        }

        float animationProportion = lifetimeTracker.Lifetime / animationEndTime;
        Vector3 position = startPosition + (endPosition - startPosition) * animationProportion;
        position.y += trajectoryCurve.Evaluate(lifetimeTracker.Lifetime);
        transform.position = position;
    }
}
