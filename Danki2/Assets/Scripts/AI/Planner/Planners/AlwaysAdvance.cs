using UnityEngine;

[Planner("Always Advance", new string[] { "chance" })]
public class AlwaysAdvance : Planner
{
    private float _chance;

    public override void Initilize()
    {
        _chance = Args[0];
    }

    public override Agenda Plan(Actor actor, Agenda previousAgenda)
    {
        Agenda agenda = new Agenda();

        if (Random.Range(0f, 1f) < _chance)
        {
            agenda.Add(AIAction.Advance, true);
        }

        return agenda;
    }
}
