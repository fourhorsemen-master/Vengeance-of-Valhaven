using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar = null;
    public Actor actor;

    private void Update()
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

        if (actor.HealthManager.Health <= 0)
        {
            _healthBar.canvas.enabled = false;
            return;
        }

        // Update the remaining health display
        _healthBar.transform.localScale = new Vector3((float)actor.HealthManager.Health / actor.HealthManager.MaxHealth, 1f, 1f);
    }
}
