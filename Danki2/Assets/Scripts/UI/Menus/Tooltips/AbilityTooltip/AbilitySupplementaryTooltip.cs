using UnityEngine;
using UnityEngine.UI;

public class AbilitySupplementaryTooltip : MonoBehaviour
{
    [SerializeField]
    private Text description = null;

    public void Setup(Keyword keyword)
    {
        description.text = $"{TextUtils.BoldText(KeywordLookup.Instance.GetDisplayName(keyword) + ":")} {KeywordLookup.Instance.GetDescription(keyword)}";
    }
}
