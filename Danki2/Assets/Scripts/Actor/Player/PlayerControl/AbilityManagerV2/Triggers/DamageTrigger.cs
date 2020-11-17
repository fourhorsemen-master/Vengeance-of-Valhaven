internal class DamageTrigger : SubjectTrigger
{

    public DamageTrigger(Player player)
        : base(player.HealthManager.DamageSubject)
    {
    }
}