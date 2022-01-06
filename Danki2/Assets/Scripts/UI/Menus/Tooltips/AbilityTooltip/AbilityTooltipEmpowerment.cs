using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltipEmpowerment : MonoBehaviour
{
    [SerializeField]
    private Image icon = null;

    [SerializeField]
    private Text title = null;

    public void SetEmpowerment(Empowerment empowerment)
    {
        icon.sprite = EmpowermentLookup.Instance.GetSprite(empowerment);
        title.text = EmpowermentLookup.Instance.GetDisplayName(empowerment);
    }
}
