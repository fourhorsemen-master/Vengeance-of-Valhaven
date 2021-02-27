using UnityEngine;
using UnityEngine.UI;

public class AbilitySupplementaryTooltip : MonoBehaviour
{
    [SerializeField]
    private Text description = null;

    public void Setup(Keyword keyword)
    {
        description.text = $"<b>{KeywordLookup.Instance.GetDisplayName(keyword)}:</b> {KeywordLookup.Instance.GetDescription(keyword)}";
    }
}
