using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthBar = null;
    [SerializeField]
    private Image damageLagBar = null;

    private const float minLagDecrement = 0.2f;
    private const float maxLagDecrement = 1f;

    public Actor actor;
    private float previousHealthProportion;
    private float recentDamage = 0f;
    private float barWidth;

    private void Start()
    {
        damageLagBar.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 1f, 0f);
        barWidth = healthBar.rectTransform.sizeDelta.x;
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
        healthBar.rectTransform.sizeDelta = new Vector2(healthProportion * barWidth, healthBar.rectTransform.sizeDelta.y);

        // Decrement recent damage if there is any.
        float decrement = Mathf.Lerp(minLagDecrement, maxLagDecrement, recentDamage);
        recentDamage = Mathf.Max(0f, recentDamage - (Time.deltaTime * decrement));
        
        // Adjust the recent damage by any damage or healing in the current frame (cannot become less than 0). Note: -ve means healing.
        recentDamage = Mathf.Max(0f, recentDamage + previousHealthProportion - healthProportion);

        damageLagBar.rectTransform.SetInsetAndSizeFromParentEdge(
            RectTransform.Edge.Left,
            healthProportion * barWidth,
            recentDamage * barWidth
        );

        previousHealthProportion = healthProportion;
    }
}
