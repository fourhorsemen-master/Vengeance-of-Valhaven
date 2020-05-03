[Planner("Retreat when wounded", new string[] { "Retreat Duration" })]
public class RetreatWhenWounded : Planner
{
    private int retreatCount = 0;
    private bool isRetreating = false;
    private float retreatDuration;

    public override void Initialize()
    {
        retreatDuration = Args[0];
    }

    public override Agenda Plan(Actor actor, Agenda previousAgenda)
    {
        Agenda agenda = new Agenda();

        // Find Target Behaviour
        if (!actor.Target)
        {
            agenda.Add(AIAction.FindTarget, true);
            return agenda;
        }

        // Retreat Behaviour
        if (isRetreating)
        {
            agenda.Add(AIAction.Retreat, true);
            return agenda;
        }

        int maxHealth = actor.GetStat(Stat.MaxHealth);
        if (
            (retreatCount < 1 && actor.HealthManager.Health < 0.5 * maxHealth) 
            || (retreatCount < 2 && actor.HealthManager.Health < 0.2 * maxHealth)
        )
        {
            agenda.Add(AIAction.Retreat, true);
            retreatCount++;
            isRetreating = true;
            actor.WaitAndAct(retreatDuration, () => { isRetreating = false; });
            return agenda;
        }

        // Evading logic here

        // Attack and Advance Behaviours
        agenda.Add(AIAction.Advance, true);
        agenda.Add(AIAction.Attack, true);
        return agenda;
    }
}
