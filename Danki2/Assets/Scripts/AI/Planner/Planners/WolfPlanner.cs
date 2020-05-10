using UnityEngine;

/// <summary>
/// With the wolf planner, we initially patrol randomly and look for targets.
/// On finding a target, we cycle between attacking (up to 3 times) and then circling the target for a configured period.
/// On taking damage that takes us below certain health amounts, we retreat for a configured period before re-engaging.
/// </summary>
[Planner("Wolf Planner", new string[] { "Retreat Duration", "Circle Duration", "Attacks Per Engagement" })]
public class WolfPlanner : Planner
{
    private const float FirstRetreatHealth = 0.5f;
    private const float SecondRetreatHealth = 0.2f;

    private int retreatCount = 0;
    private float retreatDuration;
    private float circleDuration;
    private int attacksPerEngagement;
    private int timesAttacked = 0;
    private PhaseManager<WolfPlannerPhase> phaseManager;

    public override void Initialize()
    {
        retreatDuration = Args[0];
        circleDuration = Args[1];
        attacksPerEngagement = (int)Mathf.Floor(Args[2]);
    }

    public override void Setup(Actor actor)
    {
        phaseManager = new PhaseManager<WolfPlannerPhase>(actor, WolfPlannerPhase.Patrol, () => timesAttacked = 0)
            .WithTransition(WolfPlannerPhase.Patrol, WolfPlannerPhase.Engage)
            .WithTransition(WolfPlannerPhase.Patrol, WolfPlannerPhase.Retreat)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Patrol)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Retreat)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Circle)
            .WithTransition(WolfPlannerPhase.Retreat, WolfPlannerPhase.Patrol)
            .WithAutoTransition(WolfPlannerPhase.Retreat, WolfPlannerPhase.Engage, retreatDuration, retreatDuration/2)
            .WithTransition(WolfPlannerPhase.Circle, WolfPlannerPhase.Patrol)
            .WithAutoTransition(WolfPlannerPhase.Circle, WolfPlannerPhase.Engage, circleDuration, circleDuration / 2)
            .WithTransition(WolfPlannerPhase.Circle, WolfPlannerPhase.Retreat);

        actor.InstantCastService.CastSubject.Subscribe(() => timesAttacked += 1);
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

        float healthProportion = (float) actor.HealthManager.Health / actor.HealthManager.MaxHealth;
        if (
            (retreatCount < 1 && healthProportion < FirstRetreatHealth) 
            || (retreatCount < 2 && healthProportion < SecondRetreatHealth)
        )
        {
            retreatCount++;
            phaseManager.Transition(WolfPlannerPhase.Retreat);
        }

        if (timesAttacked >= attacksPerEngagement)
        {
            phaseManager.Transition(WolfPlannerPhase.Circle);
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
