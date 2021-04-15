using UnityEngine;

[Ability(AbilityReference.Consume, new []{ "Vampiric" })]
public class Consume : Cast
{
    public Consume(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Debug.Log("Ending consume");
    }
}
