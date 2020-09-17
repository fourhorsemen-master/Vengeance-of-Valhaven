using UnityEngine;

public class ParryVisual : MonoBehaviour
{
    public static void Create(ParryVisual prefab, Transform transform)
    {
        Instantiate(prefab, transform);
    }
}
