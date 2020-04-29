using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthBar = null;
    [SerializeField]
    private Image damageLagBar = null;

    private const float damageLagBarOffset = 0.5f;
    private const float recentDamageIncrement = 1f;
    private const float recentDamageIncrementScalar = 5f;

    public Actor actor;
    private float previousHealthProportion;
    private float recentDamage = 0f;

    private void Start()
    {
        damageLagBar.transform.localScale = new Vector3(0f, 1f, 1f);
    }

    private void Update()
    {
        if (healthBar == null)
        {
            Debug.LogError("No image found for health bar");
            return;
        }

        if (healthBar.canvas == null)
        {
            return;
        }

        if (actor.HealthManager.Health <= 0)
        {
            healthBar.canvas.enabled = false;
            return;
        }

        // Set the health bar to the correct width and position the damage lag bar at the end of the health bar.
        float healthProportion = (float)actor.HealthManager.Health / actor.HealthManager.MaxHealth;        
        healthBar.transform.localScale = new Vector3(healthProportion, 1f, 1f);
        damageLagBar.rectTransform.localPosition = new Vector3(healthProportion - damageLagBarOffset, 0f, 0f);
        
        // Tick the recent damage down at the non-linear rate set by the damage increment constants (cannot become less than 0).
        recentDamage = Mathf.Max(
            0f, 
            recentDamage - (Time.deltaTime * (recentDamage + recentDamageIncrement) / recentDamageIncrementScalar)
        );
        
        // Adjust the recent damage by any damage or healing in the current frame (cannot become less than 0). Note: -ve means healing.
        recentDamage = Mathf.Max(
            0f, 
            recentDamage + previousHealthProportion - healthProportion
        );
        
        // Set the damage lag bar to the correct width.
        damageLagBar.transform.localScale = new Vector3(recentDamage, 1f, 1f);

        previousHealthProportion = healthProportion;
    }
}
