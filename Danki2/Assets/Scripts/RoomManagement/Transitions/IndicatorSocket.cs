using UnityEngine;

public class IndicatorSocket : MonoBehaviour
{
    [SerializeField] private GameObject cylinder = null;
    [SerializeField] private GameObject directionIndicator = null;

    private void Start()
    {
        Destroy(cylinder);
        Destroy(directionIndicator);
    }
}
