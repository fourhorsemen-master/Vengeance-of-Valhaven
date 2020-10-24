using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthBar = null;
    [SerializeField]
    private Image damageLagBar = null;

    private const float minLagDecrement = 0.2f;
    private const float maxLagDecrement = 1f;

    protected abstract Actor Actor { get; }
    private float previousHealthProportion;
    private float recentDamage = 0f;
    private float barWidth;

    private void Start()
    {
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

        // Set the health bar to the correct width.
        float healthProportion = Actor.Dead
            ? 0
            : (float)Actor.HealthManager.Health / Actor.HealthManager.MaxHealth;

        healthBar.rectTransform.sizeDelta = new Vector2(healthProportion * barWidth, healthBar.rectTransform.sizeDelta.y);

        // Decrement recent damage if there is any.
        float decrement = Mathf.Lerp(minLagDecrement, maxLagDecrement, recentDamage);
        recentDamage = Mathf.Max(0f, recentDamage - (Time.deltaTime * decrement));
        
        // Adjust the recent damage by any damage or healing in the current frame (cannot become less than 0). Note: -ve means healing.
        recentDamage = Mathf.Max(0f, recentDamage + previousHealthProportion - healthProportion);

        // Set the damage lag bar offset and width.
        damageLagBar.rectTransform.SetInsetAndSizeFromParentEdge(
            RectTransform.Edge.Left,
            healthProportion * barWidth,
            recentDamage * barWidth
        );

        previousHealthProportion = healthProportion;
    }
}
