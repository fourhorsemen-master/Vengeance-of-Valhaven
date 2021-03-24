using UnityEngine;
using UnityEngine.UI;

public class AbilityOptionButtons : MonoBehaviour
{
    [SerializeField] private Button confirmButton = null;

    public Subject SkipSubject { get; } = new Subject();

    public Subject ConfirmSubject { get; } = new Subject();

    public bool Interactable { set => confirmButton.interactable = value; }

    private void OnEnable() => Interactable = false;

    public void Skip() => SkipSubject.Next();

    public void Confirm() => ConfirmSubject.Next();
}
