using UnityEngine;

public abstract class Mortal : Actor
{
    public int Health { get; private set; }

    public void ModifyHealth(int healthChange)
    {
        Health = Mathf.Min(Health + healthChange, GetStat(Stat.MaxHealth));
    }

    protected override void Start()
    {
        base.Start();

        Health = GetStat(Stat.MaxHealth);
    }

    protected override void Update()
    {
        base.Update();

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    protected abstract void OnDeath();
}
