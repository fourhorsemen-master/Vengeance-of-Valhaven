using UnityEngine;
using UnityEngine.UI;

public class AbilitySupplementaryTooltip : MonoBehaviour
{
    [SerializeField]
    private Text title = null;

    [SerializeField]
    private Text description = null;

    public void Setup(Keyword keyword)
    {
        title.text = KeywordLookup.Instance.GetDisplayName(keyword);
        description.text = KeywordLookup.Instance.GetDescription(keyword);
    }
}
