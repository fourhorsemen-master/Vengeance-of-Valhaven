using UnityEngine;

public abstract class Mortal : Actor
{
    public int Health { get; private set; }

    public bool Dead { get; private set; }

    public void ModifyHealth(int healthChange)
    {
        if (Dead) return;

        Health = Mathf.Min(Health + healthChange, GetStat(Stat.MaxHealth));
    }

    protected override void Start()
    {
        base.Start();

        Health = GetStat(Stat.MaxHealth);
        Dead = false;
    }

    protected override void Update()
    {
        base.Update();

        if (Health <= 0 && !Dead)
        {
            OnDeath();
            Dead = true;
        }
    }

    protected abstract void OnDeath();
}
