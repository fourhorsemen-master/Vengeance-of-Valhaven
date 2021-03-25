using UnityEngine;
using UnityEngine.UI;

public class AbilityOptionPanel : MonoBehaviour
{
    [SerializeField] private Image selectedIndicator = null;
    [SerializeField] private Image image = null;
    [SerializeField] private Text text = null;

    public AbilityReference AbilityReference { get; private set; }

    public Subject OnClickSubject { get; } = new Subject();

    public Subject OnPointerEnterSubject { get; } = new Subject();

    public Subject OnPointerExitSubject { get; } = new Subject();

    public bool Selected
    {
        get => selectedIndicator.enabled;
        set => selectedIndicator.enabled = value;
    }

    private void OnEnable()
    {
        Selected = false;
    }

    public void Initialise(AbilityReference abilityReference)
    {
        AbilityReference = abilityReference;
        image.sprite = AbilityIconManager.Instance.GetIcon(abilityReference);
        text.text = AbilityLookup.Instance.GetAbilityDisplayName(abilityReference);
    }

    public void Click() => OnClickSubject.Next();

    public void PointerEnter() => OnPointerEnterSubject.Next();

    public void PointerExit() => OnPointerExitSubject.Next();
}
