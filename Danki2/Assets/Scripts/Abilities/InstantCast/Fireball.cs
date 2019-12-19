using UnityEngine;

public class Fireball : InstantCast
{
    private static readonly float FIREBALL_SPEED = 5;
    private readonly AbilityContext _context;

    public Fireball(AbilityContext context) : base(context)
    {
        _context = context;
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(_context.Owner, OnCollision, FIREBALL_SPEED, position, rotation);
    }

    public void OnCollision(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Mortal>(out var mortal))
        {
            var strength = _context.Owner.GetStat(Stat.Strength);
            mortal.ModifyHealth(-strength);
        }
    }
}
