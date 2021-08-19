using UnityEngine;
using UnityEngine.UI;

public class AbilityOptionPanel : MonoBehaviour
{
    [SerializeField] private Image selectedIndicator = null;
    [SerializeField] private Image highlightedIndicator = null;
    [SerializeField] private Image image = null;
    [SerializeField] private Image frame = null;
    [SerializeField] private Text text = null;

    public Ability2 Ability { get; private set; }

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

    public void Initialise(Ability2 ability)
    {
        Ability = ability;

        RarityData rarityData = RarityLookup.Instance.Lookup[AbilityLookup2.Instance.GetRarity(ability)];

        image.sprite = AbilityLookup2.Instance.GetIcon(ability);
        frame.sprite = rarityData.Frame;
        frame.color = rarityData.Colour;
        text.text = AbilityLookup2.Instance.GetDisplayName(ability);
        text.color = rarityData.Colour;
    }

    public void Click() => OnClickSubject.Next();

    public void PointerEnter() => OnPointerEnterSubject.Next();

    public void PointerExit() => OnPointerExitSubject.Next();
}
