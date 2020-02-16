public class ShieldBash : InstantCast
{
    private readonly AbilityContext _context;

    public ShieldBash(AbilityContext context) : base(context)
    {
        this._context = context;
    }

    public override void Cast()
    {
        _context.Owner.AddActiveEffect(new Slow(0.5f), 1.5f);
    }
}
