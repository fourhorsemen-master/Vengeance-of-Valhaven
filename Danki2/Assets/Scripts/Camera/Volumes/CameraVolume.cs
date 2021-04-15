using System;
using UnityEngine;

[Serializable]
public class CameraTransformLookup : SerializableEnumDictionary<Pole, Transform>
{
    public CameraTransformLookup(Transform defaultValue) : base(defaultValue) {}
    public CameraTransformLookup(Func<Transform> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class PoleColorLookup : SerializableEnumDictionary<Pole, Color>
{
    public PoleColorLookup(Color defaultValue) : base(defaultValue) {}
    public PoleColorLookup(Func<Color> defaultValueProvider) : base(defaultValueProvider) {}
}

public class CameraVolume : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer = null;
    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }

    [SerializeField]
    private CameraTransformLookup cameraTransformLookup = new CameraTransformLookup(defaultValue: null);
    public CameraTransformLookup CameraTransformLookup => cameraTransformLookup;

    [SerializeField]
    private PoleColorLookup poleColorLookup = new PoleColorLookup(Color.white);
    public PoleColorLookup PoleColorLookup => poleColorLookup;

    [SerializeField]
    private bool overrideSmoothFactor = false;
    public bool OverrideSmoothFactor { get => overrideSmoothFactor; set => overrideSmoothFactor = value; }

    [SerializeField]
    private float smoothFactorOverride = 0;
    public float SmoothFactorOverride { get => smoothFactorOverride; set => smoothFactorOverride = value; }

    [SerializeField]
    private bool turnOffStaticAbilityUI = true;
    public bool TurnOffStaticAbilityUI { get => turnOffStaticAbilityUI; set => turnOffStaticAbilityUI = value; }

    [SerializeField]
    private bool turnOffStaticHealthBarUI = true;
    public bool TurnOffStaticHealthBarUI { get => turnOffStaticHealthBarUI; set => turnOffStaticHealthBarUI = value; }

    private Pole cameraOrientation;

    private void OnDrawGizmos()
    {
        EnumUtils.ForEach<Pole>(p => GizmoUtils.DrawArrow(
            cameraTransformLookup[p].position,
            cameraTransformLookup[p].forward,
            poleColorLookup[p]
        ));
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

        CustomCamera.Instance.OverrideDesiredTransform(
            cameraTransformLookup[cameraOrientation],
            overrideSmoothFactor ? smoothFactorOverride : (float?) null
        );

        if (turnOffStaticAbilityUI) StaticAbilityUI.Instance.OverrideVisibility(0);
        if (turnOffStaticHealthBarUI) StaticHealthBarUI.Instance.OverrideVisibility(0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;

        CustomCamera.Instance.RemoveTransformOverride();

        if (turnOffStaticAbilityUI) StaticAbilityUI.Instance.RemoveVisibilityOverride();
        if (turnOffStaticHealthBarUI) StaticHealthBarUI.Instance.RemoveVisibilityOverride();
    }

    private bool IsPlayer(Collider other) => other.CompareTag(Tag.Player);
}
