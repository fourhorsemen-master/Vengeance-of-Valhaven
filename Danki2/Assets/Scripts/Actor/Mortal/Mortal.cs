using UnityEngine;
using UnityEngine.UI;

public abstract class Mortal : Actor
{
    public int Health { get; private set; }

    public Image HealthBar;

    public bool Dead { get; private set; }

    public void ModifyHealth(int healthChange)
    {
        if (Dead) return;

        Health = Mathf.Min(Health + healthChange, (int)GetStat(Stat.MaxHealth));
    }

    protected override void Awake()
    {
        base.Awake();

        // Note that if health is less than 1 you'll die on frame 1.
        Health = (int)GetStat(Stat.MaxHealth);
        Dead = false;
    }

    protected override void Update()
    {
        base.Update();
        UpdateHealthBar();

        if (Health <= 0 && !Dead)
        {
            OnDeath();
            Dead = true;
        }
    }

    private void UpdateHealthBar()
    {
        if (HealthBar == null || HealthBar.canvas == null)
        {
            return;
        }

        if (Health > 0)
        {
            var lookAtPosition = 2 * HealthBar.canvas.transform.position - Camera.main.transform.position;
            lookAtPosition.x = HealthBar.canvas.transform.position.x;
            HealthBar.canvas.transform.LookAt(lookAtPosition);
            float maxHealth = GetStat(Stat.MaxHealth);
            HealthBar.transform.localScale = new Vector3(Health / maxHealth, 1f, 1f);
        }
        else
        {
            HealthBar.canvas.enabled = false;
        }
    }

    protected abstract void OnDeath();
}
