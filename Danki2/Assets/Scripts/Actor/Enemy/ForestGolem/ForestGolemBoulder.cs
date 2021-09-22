using System;
using UnityEngine;

public class ForestGolemBoulder : MonoBehaviour
{
    [SerializeField] private LifetimeTracker lifetimeTracker = null;
    [SerializeField] private AnimationCurve trajectoryCurve = null;
    [SerializeField] private float smashScaleFactor = 0;

    private float animationEndTime;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Action<float> callback;

    private bool finished = false;

    public static void Create(ForestGolemBoulder prefab, Vector3 startPosition, Vector3 endPosition, Action<float> callback)
    {
        ForestGolemBoulder forestGolemBoulder = Instantiate(prefab, startPosition, Quaternion.identity);
        forestGolemBoulder.startPosition = startPosition;
        forestGolemBoulder.endPosition = endPosition;
        forestGolemBoulder.callback = callback;
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
            callback(animationEndTime);
            finished = true;
            return;
        }

        float animationProportion = lifetimeTracker.Lifetime / animationEndTime;
        Vector3 position = startPosition + (endPosition - startPosition) * animationProportion;
        position.y += trajectoryCurve.Evaluate(lifetimeTracker.Lifetime);
        transform.position = position;
    }
}
