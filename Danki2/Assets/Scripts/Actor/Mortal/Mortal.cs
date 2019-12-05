using UnityEngine;

public abstract class Mortal : Actor
{
    private StatsManager _statsManager;

    protected override void Start()
    {
        base.Start();

        _statsManager = gameObject.GetComponent<StatsManager>();

        Health = _statsManager[Stat.MaxHealth];
        Dead = false;
    }
    public int Health { get; private set; }

    public bool Dead { get; private set; }

    protected override void Update()
    {
        base.Update();

        if (Health <= 0 && !Dead)
        {
            OnDeath();
            Dead = true;
        }
    }

    public void ModifyHealth(int healthChange)
    {
        if (Dead) return;

        Health = Mathf.Min(Health + healthChange, _statsManager[Stat.MaxHealth]);
    }

    protected abstract void OnDeath();
}
