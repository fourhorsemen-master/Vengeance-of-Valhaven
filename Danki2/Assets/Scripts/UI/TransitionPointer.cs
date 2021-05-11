using System;
using UnityEngine;

public class TransitionPointer : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    public Vector3 TransitionPosition { private get; set; }

    public static void Create(TransitionPointer prefab, Vector3 transitionPosition)
    {
        Instantiate(prefab).TransitionPosition = transitionPosition;
    }

    private void Start() => SetPosition();

    private void Update() => SetPosition();

    private void SetPosition()
    {
        if (CustomCamera.Instance.PointInViewport(TransitionPosition /* + offset */))
        {
            Hover();
        }
        else
        {
            Point();
        }
    }

    private void Hover() { }

    private void Point() { }
}
