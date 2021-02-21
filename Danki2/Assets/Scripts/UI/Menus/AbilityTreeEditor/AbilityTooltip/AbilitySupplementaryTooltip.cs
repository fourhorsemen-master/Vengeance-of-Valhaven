using UnityEngine;
using UnityEngine.UI;

public class AbilitySupplementaryTooltip : MonoBehaviour
{
    [SerializeField]
    private Text title = null;

    [SerializeField]
    private Text description = null;

    public void Setup(SupplementaryTooltip type)
    {
        // Todo: use enum value to get text values from a lookup.
        title.text = "title";
        description.text = "description";
    }
}
