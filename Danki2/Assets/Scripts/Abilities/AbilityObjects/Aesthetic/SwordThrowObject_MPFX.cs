using UnityEngine;

public class SwordThrowObject_MPFX : MonoBehaviour
{
    private float stickTime = 1f;

    private void Start()
    {
        this.WaitAndAct(stickTime, () => Destroy(gameObject));
    }
}
