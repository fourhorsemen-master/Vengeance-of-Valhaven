using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar;
    public Mortal mortal;

    void Update()
    {
        if (_healthBar == null || _healthBar.canvas == null)
        {
            return;
        }

        if (mortal.Health <= 0)
        {
            _healthBar.canvas.enabled = false;
            return;
        }

        FaceCamera();

        // Update the remaining health display
        float maxHealth = mortal.GetStat(Stat.MaxHealth);
        _healthBar.transform.localScale = new Vector3(mortal.Health / maxHealth, 1f, 1f);
    }

    /// <summary>
    /// Rotate the health bar to face the camera, but lock the y-rotation
    /// </summary>
    private void FaceCamera()
    {
        var healthBarPosition = _healthBar.canvas.transform.position;
        var cameraPosition = Camera.main.transform.position;
        var lookAtPosition = 2 * healthBarPosition - cameraPosition;
        lookAtPosition.x = healthBarPosition.x;
        _healthBar.canvas.transform.LookAt(lookAtPosition);
    }
}
