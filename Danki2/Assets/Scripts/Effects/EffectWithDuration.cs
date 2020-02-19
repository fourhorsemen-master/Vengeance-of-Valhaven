public class EffectWithDuration
{
    public Effect Effect { get; set; }
    public float RemainingDuration { get; set; }

    public EffectWithDuration(Effect effect, float duration)
    {
        Effect = effect;
        RemainingDuration = duration;
    }
}
