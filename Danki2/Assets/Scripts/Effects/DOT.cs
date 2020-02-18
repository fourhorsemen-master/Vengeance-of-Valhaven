using UnityEngine;

class DOT : Effect
{
    private readonly float _dps;

    public DOT(float dps)
    {
        _dps = dps;
    }

    public override void Update(Actor actor)
    {
        actor.ModifyHealth(-_dps * Time.deltaTime);
    }
}

