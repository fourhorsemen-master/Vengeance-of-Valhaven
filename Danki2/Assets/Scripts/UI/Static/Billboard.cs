using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        Vector3 lookAtPosition = 2 * transform.position - Camera.main.transform.position;
        lookAtPosition.x = transform.position.x;
        transform.LookAt(lookAtPosition);
    }
}
