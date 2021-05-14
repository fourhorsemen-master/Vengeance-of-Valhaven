using UnityEngine;

public class FloatingHealthBar : HealthBar
{
    [SerializeField]
    private Diegetic diegetic = null;

    protected override Actor Actor => diegetic.Actor;
}
