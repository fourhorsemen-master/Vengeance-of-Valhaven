using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityEmpowermentContainer : MonoBehaviour
{
    [SerializeField]
    private Image empowermentPrefab = null;

    public void SetEmpowerments(SerializableGuid abilityId)
    {
        List<Empowerment> empowerments = AbilityLookup.Instance.GetEmpowerments(abilityId);

        foreach (Empowerment empowerment in empowerments)
        {
            Sprite sprite = EmpowermentLookup.Instance.GetSprite(empowerment);

            Instantiate(empowermentPrefab, transform).sprite = sprite;
        }
    }
}
