using UnityEngine;

public class AbilityInserter : MonoBehaviour
{
    [SerializeField]
    private AbilityInsertionArea topLeftArrow = null;

    [SerializeField]
    private AbilityInsertionArea topRightArrow = null;

    [SerializeField]
    private AbilityInsertionArea bottomRightArrow = null;

    [SerializeField]
    private AbilityInsertionArea bottomLeftArrow = null;

    [SerializeField]
    private AbilityInsertionArea centralArea = null;

    private void Start()
    {
        topLeftArrow.MouseUpSubject.Subscribe();
    }
}
