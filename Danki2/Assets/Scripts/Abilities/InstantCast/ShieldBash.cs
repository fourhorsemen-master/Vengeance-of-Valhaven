﻿using UnityEngine;

public class ShieldBash : InstantCast
{
    private readonly AbilityContext _context;

    public ShieldBash(AbilityContext context) : base(context)
    {
        this._context = context;
    }

    public override void Cast()
    {
        _context.Owner.AddEffect(new Slow(1.5f, 0.5f));
    }
}
