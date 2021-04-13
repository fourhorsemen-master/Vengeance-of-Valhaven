using UnityEngine;

[Ability(AbilityReference.PoisonStab)]
public class PoisonStab : Cast
{
    public PoisonStab(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Debug.Log("Poison stab end");
    }
}
