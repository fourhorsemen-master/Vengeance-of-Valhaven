using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityListingEmpowerments : MonoBehaviour
{
    [SerializeField]
    private Image abilityListingEmpowermentPrefab = null;

    public void Initialise(List<Empowerment> empowerments)
    {
        foreach(Empowerment empowerment in empowerments)
        {
            Color colour = EmpowermentLookup.Instance.GetColour(empowerment);

            Instantiate(abilityListingEmpowermentPrefab, transform).color = colour;
        }
    }
}
