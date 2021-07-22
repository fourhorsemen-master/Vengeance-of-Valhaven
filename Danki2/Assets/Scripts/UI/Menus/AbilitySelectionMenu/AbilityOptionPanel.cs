using UnityEngine;
using UnityEngine.UI;

public class AbilityOptionPanel : MonoBehaviour
{
    [SerializeField] private Image selectedIndicator = null;
    [SerializeField] private Image highlightedIndicator = null;
    [SerializeField] private Image image = null;
    [SerializeField] private Image frame = null;
    [SerializeField] private Text text = null;

    public Ability2 AbilityReference { get; private set; }

    public Subject OnClickSubject { get; } = new Subject();

    public Subject OnPointerEnterSubject { get; } = new Subject();

    public Subject OnPointerExitSubject { get; } = new Subject();

    public bool Selected
    {
        get => selectedIndicator.enabled;
        set => selectedIndicator.enabled = value;
    }

    public bool Highlighted
    {
        set => highlightedIndicator.enabled = value;
    }

    private void OnEnable()
    {
        Selected = false;
        Highlighted = false;
    }

    public void Initialise(Ability2 abilityReference)
    {
        AbilityReference = abilityReference;

        RarityData rarityData = RarityLookup.Instance.Lookup[AbilityLookup2.Instance.GetRarity(abilityReference)];

        image.sprite = AbilityLookup2.Instance.GetIcon(abilityReference);
        frame.sprite = rarityData.Frame;
        frame.color = rarityData.Colour;
        text.text = AbilityLookup2.Instance.GetDisplayName(abilityReference);
        text.color = rarityData.Colour;
    }

    public void Click() => OnClickSubject.Next();

    public void PointerEnter() => OnPointerEnterSubject.Next();

    public void PointerExit() => OnPointerExitSubject.Next();
}
