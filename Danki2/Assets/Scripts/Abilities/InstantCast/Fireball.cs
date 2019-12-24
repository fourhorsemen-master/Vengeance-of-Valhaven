using UnityEngine;

public class Fireball : InstantCast
{
    private static readonly float SPEED = 5;
    private static readonly Vector3 POSITION_TRANSFORM = new Vector3(0, 1.25f, 0);

    public Fireball(AbilityContext context) : base(context)
    {

    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position + POSITION_TRANSFORM;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(Context.Owner, OnCollision, SPEED, position, rotation);
    }

    public void OnCollision(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Mortal>(out var mortal))
        {
            var strength = Context.Owner.GetStat(Stat.Strength);
            mortal.ModifyHealth(-strength);
        }
    }
}
