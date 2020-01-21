using UnityEngine;

[Planner("Wolf Planner", new string[] { "Retreat Duration" })]
public class WolfPlanner : Planner
{
    private int _retreatCount = 0;
    private bool _isRetreating = false;
    private float _retreatTimer = 0f;
    private float _retreatDuration;

    public override void Initialize()
    {
        _retreatDuration = Args[0];
    }

    public override Agenda Plan(AI ai, Actor actor, Agenda previousAgenda)
    {
        Mortal mortal = (Mortal)actor;
        Agenda agenda = new Agenda();

        _retreatTimer -= Time.deltaTime;
        _isRetreating = _isRetreating && _retreatTimer > 0f;

        // Find Target Behaviour
        if (!ai.Target)
        {
            agenda.Add(AIAction.FindTarget, true);
            return agenda;
        }

        // Retreat Behaviour
        if (_isRetreating)
        {
            agenda.Add(AIAction.Retreat, true);
            return agenda;
        }

        int maxHealth = mortal.GetStat(Stat.MaxHealth);
        if (
            (_retreatCount < 1 && mortal.Health < 0.5 * maxHealth) 
            || (_retreatCount < 2 && mortal.Health < 0.2 * maxHealth)
        )
        {
            agenda.Add(AIAction.Retreat, true);
            _retreatCount++;
            _isRetreating = true;
            _retreatTimer = _retreatDuration;
            return agenda;
        }

        // Attack Behaviour
        agenda.Add(AIAction.Attack, true);
        return agenda;
    }
}
