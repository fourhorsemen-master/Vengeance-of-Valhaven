using UnityEngine;

public abstract class Mortal : Actor
{
    private float _health;

    public int Health => Mathf.CeilToInt(_health);

    public bool Dead { get; private set; }

    public void ModifyHealth(float healthChange)
    {
        if (Dead) return;

        _health = Mathf.Min(_health + healthChange, GetStat(Stat.MaxHealth));
    }

    protected override void Awake()
    {
        base.Awake();

        // Note that if health is less than 1 you'll die on frame 1.
        _health = GetStat(Stat.MaxHealth);
        Dead = false;
    }

    protected override void Update()
    {
        base.Update();

        if (_health <= 0f && !Dead)
        {
            OnDeath();
            Dead = true;
        }
    }

    protected abstract void OnDeath();
}
