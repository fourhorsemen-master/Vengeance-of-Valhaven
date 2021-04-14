using UnityEngine;

public class CameraVolume : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer = null;

    [SerializeField] private Transform northTransform = null;
    [SerializeField] private Transform eastTransform = null;
    [SerializeField] private Transform southTransform = null;
    [SerializeField] private Transform westTransform = null;

    [SerializeField] private bool overrideSmoothFactor = false;
    [SerializeField] private float smoothFactorOverride = 1;

    private Pole cameraOrientation;

    private void OnDrawGizmos()
    {
        GizmoUtils.DrawArrow(northTransform.position, northTransform.forward, Color.blue);
        GizmoUtils.DrawArrow(eastTransform.position, eastTransform.forward, Color.red);
        GizmoUtils.DrawArrow(southTransform.position, southTransform.forward, Color.green);
        GizmoUtils.DrawArrow(westTransform.position, westTransform.forward, Color.yellow);
    }

    private void Awake()
    {
        cameraOrientation = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.CameraOrientation;
    }

    private void Start()
    {
        meshRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        switch (cameraOrientation)
        {
            case Pole.North:
                CustomCamera.Instance.OverrideDesiredTransform(northTransform, overrideSmoothFactor ? smoothFactorOverride : -1);
                break;
            case Pole.East:
                CustomCamera.Instance.OverrideDesiredTransform(eastTransform, overrideSmoothFactor ? smoothFactorOverride : -1);
                break;
            case Pole.South:
                CustomCamera.Instance.OverrideDesiredTransform(southTransform, overrideSmoothFactor ? smoothFactorOverride : -1);
                break;
            case Pole.West:
                CustomCamera.Instance.OverrideDesiredTransform(westTransform, overrideSmoothFactor ? smoothFactorOverride : -1);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other)) CustomCamera.Instance.RemoveTransformOverride();
    }

    private bool IsPlayer(Collider other) => other.CompareTag(Tag.Player);
}
