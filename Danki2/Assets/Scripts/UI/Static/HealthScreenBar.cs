public class HealthScreenBar : HealthBar
{
    protected override Actor Actor => ActorCache.Instance.Player;
}
