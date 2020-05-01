[Planner("Retreat when wounded", new string[] { "Retreat Duration", "Circle Duration", "Engage Duration" })]
public class RetreatWhenWounded : Planner
{
    private int retreatCount = 0;
    private float retreatDuration;
    private float circleDuration;
    private float engageDuration;
    private PhaseManager<WolfPlannerPhase> phaseManager;

    public override void Initialize()
    {
        retreatDuration = Args[0];
        circleDuration = Args[1];
        engageDuration = Args[2];
    }

    public override void Setup(AI ai)
    {
        phaseManager = new PhaseManager<WolfPlannerPhase>(ai, WolfPlannerPhase.Patrol)
            .WithTransition(WolfPlannerPhase.Patrol, WolfPlannerPhase.Engage)
            .WithTransition(WolfPlannerPhase.Patrol, WolfPlannerPhase.Retreat)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Patrol)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Retreat)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Circle, circleDuration)
            .WithTransition(WolfPlannerPhase.Retreat, WolfPlannerPhase.Patrol)
            .WithTransition(WolfPlannerPhase.Retreat, WolfPlannerPhase.Circle, retreatDuration)
            .WithTransition(WolfPlannerPhase.Circle, WolfPlannerPhase.Patrol)
            .WithTransition(WolfPlannerPhase.Circle, WolfPlannerPhase.Engage, engageDuration)
            .WithTransition(WolfPlannerPhase.Circle, WolfPlannerPhase.Retreat);
    }

    public override Agenda Plan(Actor actor, Agenda previousAgenda)
    {
        Agenda agenda = new Agenda();

        if (!actor.Target && phaseManager.CurrentPhase != WolfPlannerPhase.Patrol)
        {
            phaseManager.Transition(WolfPlannerPhase.Patrol);
        }
        else if (actor.Target && phaseManager.CurrentPhase == WolfPlannerPhase.Patrol)
        {
            phaseManager.Transition(WolfPlannerPhase.Engage);
        }

        int maxHealth = actor.GetStat(Stat.MaxHealth);
        if (
            (retreatCount < 1 && actor.HealthManager.Health < 0.5 * maxHealth) 
            || (retreatCount < 2 && actor.HealthManager.Health < 0.2 * maxHealth)
        )
        {
            retreatCount++;
            phaseManager.Transition(WolfPlannerPhase.Retreat);
        }

        switch (phaseManager.CurrentPhase)
        {
            case WolfPlannerPhase.Patrol:
                agenda.Add(AIAction.FindTarget, true);
                agenda.Add(AIAction.Patrol, true);
                break;
            case WolfPlannerPhase.Engage:
                agenda.Add(AIAction.Advance, true);
                agenda.Add(AIAction.Attack, true);
                break;
            case WolfPlannerPhase.Circle:
                agenda.Add(AIAction.Evade, true);
                break;
            case WolfPlannerPhase.Retreat:
                agenda.Add(AIAction.Retreat, true);
                break;
        }

        return agenda;
    }
}
