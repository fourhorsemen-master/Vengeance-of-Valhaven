using UnityEngine;
using UnityEngine.UI;

public class DraggableHighlighter : MonoBehaviour
{
    public const float DefaultOpacity = 0f;

    public const float HoverOpacity = 0.06f;

    public const float DraggingOpacity = 0.12f;

    [SerializeField]
    private Image abilityHighlight = null;

    private DraggableHighlightState highlightState;

    public DraggableHighlightState HighlightState
    {
        get
        {
            return highlightState;
        }
        set
        {
            switch (value)
            {
                case DraggableHighlightState.Default:
                    abilityHighlight.SetOpacity(DefaultOpacity);
                    break;
                case DraggableHighlightState.Hover:
                    abilityHighlight.SetOpacity(HoverOpacity);
                    break;
                case DraggableHighlightState.Dragging:
                    abilityHighlight.SetOpacity(DraggingOpacity);
                    break;
            }

            highlightState = value;
        }
    }
}
