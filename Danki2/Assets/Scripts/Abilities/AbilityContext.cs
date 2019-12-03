using UnityEngine;

public class AbilityContext
{
    public AbilityContext(
        Actor owner,
        Vector3 targetPosition
    )
    {
        Owner = owner;
        TargetPosition = targetPosition;

    }

    public Actor Owner { get; }
    public Vector3 TargetPosition { get; }
}
