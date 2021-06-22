using System;
using UnityEngine;

public class ForestGolemBoulder : MonoBehaviour
{
    [SerializeField] private LifetimeTracker lifetimeTracker = null;
    [SerializeField] private AnimationCurve trajectory = null;
    [SerializeField] private float smashScaleFactor = 0;

    private float animationEndTime;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Action callback;

    private bool finished = false;

    public static void Create(ForestGolemBoulder prefab, Vector3 startPosition, Vector3 endPosition, Action callback)
    {
        ForestGolemBoulder forestGolemBoulder = Instantiate(prefab, startPosition, Quaternion.identity);
        forestGolemBoulder.startPosition = startPosition;
        forestGolemBoulder.endPosition = endPosition;
        forestGolemBoulder.callback = callback;
    }

    private void Start()
    {
        animationEndTime = AnimationCurveUtils.GetEndTime(trajectory);
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
            return;
        }

        float animationProportion = lifetimeTracker.Lifetime / animationEndTime;
        Vector3 position = startPosition + (endPosition - startPosition) * animationProportion;
        position.y += trajectory.Evaluate(lifetimeTracker.Lifetime);
        transform.position = position;
    }
}
