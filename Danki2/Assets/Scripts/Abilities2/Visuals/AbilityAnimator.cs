using System;
using UnityEngine;

[Serializable]
public class AbilityTypeToAnimationTypeDictionary : SerializableEnumDictionary<AbilityType2, AbilityAnimationType>
{
    public AbilityTypeToAnimationTypeDictionary(AbilityAnimationType defaultValue) : base(defaultValue) {}
    public AbilityTypeToAnimationTypeDictionary(Func<AbilityAnimationType> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityTypeToAnimationSpeedCurveDictionary : SerializableEnumDictionary<AbilityType2, AnimationCurve>
{
    public AbilityTypeToAnimationSpeedCurveDictionary(AnimationCurve defaultValue) : base(defaultValue) { }
    public AbilityTypeToAnimationSpeedCurveDictionary(Func<AnimationCurve> defaultValueProvider) : base(defaultValueProvider) { }
}

public class AbilityAnimator : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private AbilityType2? currentAbilityType = null;
    private float? currentNormalizedTime = null;

    [SerializeField, HideInInspector]
    public AbilityTypeToAnimationTypeDictionary abilityTypeToAnimationTypeDictionary =
        new AbilityTypeToAnimationTypeDictionary(AbilityAnimationType.None);

    [SerializeField, HideInInspector]
    public AbilityTypeToAnimationSpeedCurveDictionary abilityTypeToAnimationSpeedCurveDictionary =
        new AbilityTypeToAnimationSpeedCurveDictionary(() => new AnimationCurve());

    private void Start()
    {
        player.AbilityService.AbilityEventSubject
            .Where(x => x.CastEvent == CastEvent.Cast)
            .Subscribe(HandleAnimation);
    }
    private void Update()
    {
        if (!currentAbilityType.HasValue) return;

        float normalizedtime = player.AnimController.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (currentNormalizedTime.HasValue && normalizedtime < currentNormalizedTime)
        {
            player.AnimController.speed = 1f;
            currentAbilityType = null;
            return;
        }

        currentNormalizedTime = currentNormalizedTime.HasValue ? normalizedtime : 0f;

        player.AnimController.speed = abilityTypeToAnimationSpeedCurveDictionary[currentAbilityType.Value]
            .Evaluate(normalizedtime);
    }

    private void HandleAnimation(AbilityCastInformation abilityCastInformation)
    {
        AbilityType2 type = AbilityLookup2.Instance.GetAbilityType(abilityCastInformation.AbilityId);
        AbilityAnimationType abilityAnimationType = abilityTypeToAnimationTypeDictionary[type];
        string animationState = AnimationStringLookup.LookupTable[abilityAnimationType];
        player.AnimController.Play(animationState);

        currentAbilityType = type;
        currentNormalizedTime = null;
    }
}
