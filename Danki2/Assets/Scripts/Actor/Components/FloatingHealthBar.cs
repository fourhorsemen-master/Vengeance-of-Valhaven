using UnityEngine;

public class FloatingHealthBar : HealthBar
{
    [SerializeField]
    private Actor actor = null;

    protected override Actor Actor => actor;
}
