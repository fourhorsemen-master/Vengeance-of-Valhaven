using UnityEngine;
using UnityEngine.UI;

public class SupplementaryTooltip : MonoBehaviour
{
    [SerializeField]
    private Text description = null;

    public void Setup(string title, string body)
    {
        description.text = $"{TextUtils.BoldText(title + ":")} {body}";
    }
}
