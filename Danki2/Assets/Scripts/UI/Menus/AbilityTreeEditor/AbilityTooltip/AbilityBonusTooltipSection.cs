using UnityEngine;
using UnityEngine.UI;

public class AbilityBonusTooltipSection : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text descriptionText = null;

    public void Initialise(AbilityBonusData bonus)
    {
        titleText.text = $"Bonus: {bonus.DisplayName}";
        descriptionText.text = bonus.Tooltip;
    }

    public float GetSectionHeight()
    {
        descriptionText.rectTransform.sizeDelta = new Vector2(
            descriptionText.rectTransform.sizeDelta.x,
            descriptionText.preferredHeight
        );

        Debug.Log(descriptionText.preferredHeight);
        this.NextFrame(() => Debug.Log(descriptionText.preferredHeight));

        float newHeight = descriptionText.preferredHeight + 30f;

        rectTransform.sizeDelta = new Vector2(
            rectTransform.sizeDelta.x,
            newHeight
        );

        return newHeight + 8f;
    }
}
