using UnityEngine;

public static class GizmoUtils
{
    public static void DrawArrow(Vector3 position, Vector3 direction, Color? color = null)
    {
        Gizmos.color = color ?? Color.white;

        Gizmos.DrawRay(position, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 20.0f, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 20.0f, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(position + direction, right * 0.25f);
        Gizmos.DrawRay(position + direction, left * 0.25f);
    }
}
