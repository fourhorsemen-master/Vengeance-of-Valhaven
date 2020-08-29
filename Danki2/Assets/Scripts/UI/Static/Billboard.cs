using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// <summary>
    /// Note: This is LateUpdate() rather than Update() so that it happens after any position updates to parent game
    ///       objects that this component is on. Otherwise this could be called before the parent game object's Update()
    ///       and, if it has moved, would cause this game object to be out of position for a frame.
    /// </summary>
    private void LateUpdate()
    {
        Vector3 lookAtPosition = 2 * transform.position - Camera.main.transform.position;
        lookAtPosition.x = transform.position.x;
        transform.LookAt(lookAtPosition);
    }
}
