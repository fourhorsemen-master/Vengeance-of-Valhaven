using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar = null;
    public Actor actor;

    void Update()
    {
        if (_healthBar == null)
        {
            Debug.LogError("No image found for health bar");
            return;
        }

        if (_healthBar.canvas == null)
        {
            return;
        }

        if (actor.Health <= 0)
        {
            _healthBar.canvas.enabled = false;
            return;
        }

        // Update the remaining health display
        float maxHealth = actor.GetStat(Stat.MaxHealth);
        _healthBar.transform.localScale = new Vector3(actor.Health / maxHealth, 1f, 1f);
    }
}
