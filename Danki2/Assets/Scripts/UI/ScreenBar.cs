using UnityEngine;

public class ScreenBar : MonoBehaviour
{
    protected void SetWidth(float width)
    {
        transform.localScale = new Vector3(width, 1f, 1f);
    }
}
