using UnityEngine;

[Ability(AbilityReference.Blink)]
public class Blink : InstantCast
{
    public Blink(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        SuccessFeedbackSubject.Next(true);
        Owner.transform.position = floorTargetPosition;
    }
}
