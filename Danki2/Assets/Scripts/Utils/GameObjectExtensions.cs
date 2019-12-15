using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class GameObjectExtensions
    {
        public static bool TryGetComponent<T>(this GameObject @object, out T component)
            where T : MonoBehaviour
        {
            component = @object.GetComponent<T>();
            return component != null;
        }
    }
}
