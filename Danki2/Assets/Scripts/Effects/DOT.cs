class DOT : Effect
{
    private readonly float _dps;

    public DOT(float duration, float dps) : base(duration)
    {
        _dps = dps;
    }

    protected override void UpdateAction(Actor actor, float deltaTime)
    {
        var mortal = (Mortal)actor;
        mortal.ModifyHealth(-_dps * deltaTime);
    }
}

