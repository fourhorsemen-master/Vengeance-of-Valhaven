using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar = null;
    public Mortal mortal;

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

        if (mortal.Health <= 0)
        {
            _healthBar.canvas.enabled = false;
            return;
        }

        // Update the remaining health display
        float maxHealth = mortal.GetStat(Stat.MaxHealth);
        _healthBar.transform.localScale = new Vector3(mortal.Health / maxHealth, 1f, 1f);
    }
}
