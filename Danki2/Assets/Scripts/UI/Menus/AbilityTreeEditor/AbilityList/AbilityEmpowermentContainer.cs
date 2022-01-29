using UnityEngine;
using UnityEngine.UI;

public class AbilityEmpowermentContainer : MonoBehaviour
{
    [SerializeField]
    private Image empowermentPrefab = null;

    public void SetEmpowerments(Ability ability)
    {
        foreach (Empowerment empowerment in ability.Empowerments)
        {
            Sprite sprite = EmpowermentLookup.Instance.GetSprite(empowerment);

            Instantiate(empowermentPrefab, transform).sprite = sprite;
        }
    }
}
