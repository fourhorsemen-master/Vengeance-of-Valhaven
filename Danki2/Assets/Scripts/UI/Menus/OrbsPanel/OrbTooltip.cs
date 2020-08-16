using UnityEngine;
using UnityEngine.UI;

public class OrbTooltip : Tooltip<OrbTooltip>
{
    [SerializeField]
    private Text description = null;

    private bool heightInitialised = false;

    public void Activate(OrbType orbType)
    {
        ActivateTooltip();

        description.text = OrbLookup.Instance.GetDisplayName(orbType);

        if (!heightInitialised)
        {
            this.NextFrame(SetHeight);
            heightInitialised = true;
        }
        else
        {
            SetHeight();
        }
    }

    public void Deactivate() => DeactivateTooltip();

    private void SetHeight()
    {
        description.rectTransform.sizeDelta = new Vector2(
            description.rectTransform.sizeDelta.x,
            description.preferredHeight
        );

        tooltipPanel.sizeDelta = new Vector2(
            tooltipPanel.sizeDelta.x,
            description.preferredHeight + 12f
        );
    }
}
