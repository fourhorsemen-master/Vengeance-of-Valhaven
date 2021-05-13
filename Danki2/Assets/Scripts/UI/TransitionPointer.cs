
using UnityEngine;
using UnityEngine.UI;

public class TransitionPointer : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Image image = null;

    private const float edgeOffset = 20f;

    private Vector3 transitionPosition;

    public static void Create(TransitionPointer prefab, Vector3 transitionPosition)
    {
        Instantiate(prefab).SetTransitionPosition(transitionPosition);
    }

    private void Start() => SetPosition();

    private void Update() => SetPosition();

    public void SetTransitionPosition(Vector3 transitionPosition)
    {
        this.transitionPosition = transitionPosition;
    }

    private void SetPosition()
    {
        if (CustomCamera.Instance.PointInViewport(transitionPosition))
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            Point();
        }
    }

    private void Point()
    {
        Vector3 viewportPosition = CustomCamera.Instance.WorldToViewportPoint(transitionPosition);

        Vector3 clampedPosition = ClampToScreenEdge(viewportPosition);

        float pointerX = Mathf.Clamp(clampedPosition.x * Screen.width, edgeOffset, Screen.width - edgeOffset);
        float pointerY = Mathf.Clamp(clampedPosition.y * Screen.height, edgeOffset, Screen.height - edgeOffset);

        rectTransform.position = new Vector3(pointerX, pointerY);
        rectTransform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(clampedPosition.x - 0.5f, clampedPosition.y - 0.5f, 0f));
    }

    private Vector3 ClampToScreenEdge(Vector3 viewportPosition)
    {
        if (viewportPosition.z < 0) viewportPosition = -viewportPosition;

        // Get the position relative to the centre of the screen
        var adjustedPosition = viewportPosition - Vector3.one * 0.5f;

        float x, y;

        if (Mathf.Abs(adjustedPosition.x) > Mathf.Abs(adjustedPosition.y))
        {
            x = Mathf.Clamp(viewportPosition.x, 0, 1); // We're on the left or right screen edge
            y = 0.5f * adjustedPosition.y / Mathf.Abs(adjustedPosition.x) + 0.5f;
        }
        else
        {
            x = 0.5f * adjustedPosition.x / Mathf.Abs(adjustedPosition.y) + 0.5f;
            y = Mathf.Clamp(viewportPosition.y, 0, 1); // We're on the top or bottom screen edge
        }

        return new Vector3(x, y);
    }
}
