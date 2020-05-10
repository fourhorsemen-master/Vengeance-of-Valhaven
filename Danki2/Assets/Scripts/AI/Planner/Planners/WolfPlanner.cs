using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// With the wolf planner, we initially patrol randomly and look for targets.
/// On finding a target, we cycle between attacking a random number of times (within given bounds) and then evading the target for a configured period.
/// On taking damage that takes us below certain health amounts, we retreat for a configured period before re-engaging.
/// </summary>
[Planner("Wolf Planner", new string[] { "Retreat Duration", "Evade Duration", "Evade Duration Variance", "Min attacks Per Engagement", "Max attacks Per Engagement" })]
public class WolfPlanner : Planner
{
    private const float FirstRetreatHealth = 0.5f;
    private const float SecondRetreatHealth = 0.2f;

    private int retreatCount = 0;
    private float retreatDuration;
    private float evadeDuration;
    private float evadeDurationVariance;
    private int minAttacksPerEngagement;
    private int maxAttacksPerEngagement;
    private int timesAttacked = 0;
    private int attacksThisEngagement;
    private PhaseManager<WolfPlannerPhase> phaseManager;

    public override void DeserializeArgs()
    {
        retreatDuration = Args[0];
        evadeDuration = Args[1];
        evadeDurationVariance = Args[2];
        minAttacksPerEngagement = Mathf.FloorToInt(Args[3]);
        maxAttacksPerEngagement = Mathf.FloorToInt(Args[4]);
    }

    public override void OnStart(Actor actor)
    {
        ResetAttacks();
        actor.InstantCastService.CastSubject.Subscribe(() => timesAttacked += 1);

        phaseManager = new PhaseManager<WolfPlannerPhase>(actor, WolfPlannerPhase.Patrol, () => ResetAttacks())
            .WithTransition(WolfPlannerPhase.Patrol, WolfPlannerPhase.Engage)
            .WithTransition(WolfPlannerPhase.Patrol, WolfPlannerPhase.Retreat)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Patrol)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Retreat)
            .WithTransition(WolfPlannerPhase.Engage, WolfPlannerPhase.Evade)
            .WithTransition(WolfPlannerPhase.Retreat, WolfPlannerPhase.Patrol)
            .WithAutoTransition(WolfPlannerPhase.Retreat, WolfPlannerPhase.Engage, retreatDuration, retreatDuration/2)
            .WithTransition(WolfPlannerPhase.Evade, WolfPlannerPhase.Patrol)
            .WithAutoTransition(WolfPlannerPhase.Evade, WolfPlannerPhase.Engage, evadeDuration, evadeDurationVariance / 2)
            .WithTransition(WolfPlannerPhase.Evade, WolfPlannerPhase.Retreat);
    }

    private void ResetAttacks()
    {
        timesAttacked = 0;
        attacksThisEngagement = Random.Range(minAttacksPerEngagement, maxAttacksPerEngagement + 1);
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

        if (timesAttacked >= attacksThisEngagement)
        {
            phaseManager.Transition(WolfPlannerPhase.Evade);
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
            case WolfPlannerPhase.Evade:
                agenda.Add(AIAction.Evade, true);
                break;
            case WolfPlannerPhase.Retreat:
                agenda.Add(AIAction.Retreat, true);
                break;
        }

        return agenda;
    }
}
