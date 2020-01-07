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

        if (Health <= 0)
        {
            HealthBar.canvas.enabled = false;
            return;
        }

        // Rotate the health bar to face the camera, but lock the y-rotation
        var healthBarPosition = HealthBar.canvas.transform.position;
        var cameraPosition = Camera.main.transform.position;
        var lookAtPosition = 2 * healthBarPosition - cameraPosition;
        lookAtPosition.x = healthBarPosition.x;
        HealthBar.canvas.transform.LookAt(lookAtPosition);

        // Update the remaining health display
        float maxHealth = GetStat(Stat.MaxHealth);
        HealthBar.transform.localScale = new Vector3(Health / maxHealth, 1f, 1f);
    }

    protected abstract void OnDeath();
}
