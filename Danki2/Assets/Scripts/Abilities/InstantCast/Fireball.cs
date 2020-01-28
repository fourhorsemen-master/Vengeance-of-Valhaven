using UnityEngine;

public class Fireball : InstantCast
{
    private static readonly float _fireballSpeed = 5;
    private static readonly Vector3 _positionTransform = new Vector3(0, 1.25f, 0);

    public Fireball(AbilityContext context) : base(context)
    {

    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position + _positionTransform;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(Context.Owner, OnCollision, _fireballSpeed, position, rotation);
    }

    public void OnCollision(GameObject gameObject)
    {
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                return;
            }

            var strength = Context.Owner.GetStat(Stat.Strength);
            actor.ModifyHealth(-strength);
        }
    }
}
