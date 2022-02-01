using System;
using UnityEngine;

[Serializable]
public class AbilityTypeToAnimationTypeDictionary : SerializableEnumDictionary<AbilityType, AbilityAnimationType>
{
    public AbilityTypeToAnimationTypeDictionary(AbilityAnimationType defaultValue) : base(defaultValue) {}
    public AbilityTypeToAnimationTypeDictionary(Func<AbilityAnimationType> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityTypeToAnimationSpeedCurveDictionary : SerializableEnumDictionary<AbilityType, AnimationCurve>
{
    public AbilityTypeToAnimationSpeedCurveDictionary(AnimationCurve defaultValue) : base(defaultValue) { }
    public AbilityTypeToAnimationSpeedCurveDictionary(Func<AnimationCurve> defaultValueProvider) : base(defaultValueProvider) { }
}

public class AbilityAnimator : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private AbilityType? currentAbilityType = null;
    private float? currentNormalizedTime = null;

    [SerializeField, HideInInspector]
    public AbilityTypeToAnimationTypeDictionary abilityTypeToAnimationTypeDictionary =
        new AbilityTypeToAnimationTypeDictionary(AbilityAnimationType.None);

    [SerializeField, HideInInspector]
    public AbilityTypeToAnimationSpeedCurveDictionary abilityTypeToAnimationSpeedCurveDictionary =
        new AbilityTypeToAnimationSpeedCurveDictionary(() => new AnimationCurve());

    private void Start()
    {
        player.InterruptSubject.Subscribe(OnInterrupt);
    }

    private void Update()
    {
        if (!currentAbilityType.HasValue) return;

        float normalizedtime = player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (currentNormalizedTime.HasValue && normalizedtime < currentNormalizedTime)
        {
            player.Animator.speed = 1f;
            currentAbilityType = null;
            return;
        }

        currentNormalizedTime = currentNormalizedTime.HasValue ? normalizedtime : 0f;

        player.Animator.speed = abilityTypeToAnimationSpeedCurveDictionary[currentAbilityType.Value]
            .Evaluate(normalizedtime);
    }

    public void HandleAnimation(AbilityType abilityType)
    {
        AbilityAnimationType abilityAnimationType = abilityTypeToAnimationTypeDictionary[abilityType];
        string animationState = AnimationStringLookup.LookupTable[abilityAnimationType];
        player.Animator.Play(animationState);

        currentAbilityType = abilityType;
        currentNormalizedTime = null;
    }

    private void OnInterrupt()
    {
        currentAbilityType = null;
        player.Animator.speed = 1;
    }
}
