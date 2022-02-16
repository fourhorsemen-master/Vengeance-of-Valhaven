using UnityEngine;

[CreateAssetMenu(fileName = "DynamicLightingConfig", menuName = "ScriptableObjects/DynamicLightingConfig", order = 1)]
public class DynamicLightingConfig : ScriptableObject

{
    [SerializeField] private Gradient color = null;
    [SerializeField] private AnimationCurve intensity = null;
    [SerializeField] private AnimationCurve volumetricsMultiplier = null;
    [SerializeField] private AnimationCurve xAngle = null;
    [SerializeField] private AnimationCurve yAngle = null;

    public Gradient Color => color;
    public AnimationCurve Intensity => intensity;
    public AnimationCurve VolumetricsMultiplier => volumetricsMultiplier;
    public AnimationCurve XAngle => xAngle;
    public AnimationCurve YAngle => yAngle;
}
