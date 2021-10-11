using System.Collections.Generic;
using UnityEngine;

public class EmpowermentVisualManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private SwordEmpowermentVisual[] visuals = null;

    private void Start()
    {
        player.AbilityAnimationListener.ImpactSubject.Subscribe(UpdateEmpowerments);
        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyAtRoot, ResetEmpowerments);
    }

    private void UpdateEmpowerments()
    {
        ResetEmpowerments();

        List<Empowerment> empowerments = player.AbilityService.CurrentEmpowerments;

        for (int i = 0; i < empowerments.Count; i ++)
        {
            if (visuals.Length <= i)
            {
                Debug.LogWarning($"Tried to display {empowerments.Count} empowerments on the sword which is beyone the limit of {visuals.Length}.");
                break;
            }

            Color colour = EmpowermentLookup.Instance.GetColour(empowerments[i]);
            Material decalMaterial = EmpowermentLookup.Instance.GetDecalMaterial(empowerments[i]);

            visuals[i].Activate(colour, decalMaterial);
        }
    }

    private void ResetEmpowerments()
    {
        foreach (SwordEmpowermentVisual visual in visuals)
        {
            visual.Reset();
        }
    }
}
