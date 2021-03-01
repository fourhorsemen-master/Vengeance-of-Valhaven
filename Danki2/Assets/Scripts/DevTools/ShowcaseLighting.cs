using System.Collections.Generic;
using UnityEngine;

public class ShowcaseLighting : MonoBehaviour
{
    public List<GameObject> MainLights;
    public List<GameObject> FillLights;

    private GameObject CurrentActiveMainLight = null;
    private GameObject CurrentActiveFillLight = null;

    // Start is called before the first frame update
    void OnValidate()
    {
        if (MainLights.Count != FillLights.Count)
        {
            Debug.LogError("The number of main lights is not equal to the number of fill lights.");
            return;
        }

        if (MainLights.Count == 0) return;

        foreach (GameObject Light in MainLights) Light.SetActive(false);
        foreach (GameObject Light in FillLights) Light.SetActive(false);

        SetActiveLight(0);
    }

    public void NextLight()
    {
        if (MainLights.Count == 0) return;

        int index = MainLights.IndexOf(CurrentActiveMainLight);
        index = ++index % MainLights.Count;
        SetActiveLight(index);
	}

    public void PrevLight()
    {
        if (MainLights.Count == 0) return;

        int index = MainLights.IndexOf(CurrentActiveMainLight);
        index = TrueMod(--index, MainLights.Count);
        SetActiveLight(index);
	}

    public void HomeLight()
    {
        if (MainLights.Count == 0) return;

        SetActiveLight(0);
	}

    private void SetActiveLight(int NewActiveLightIndex)
	{
        if (CurrentActiveMainLight != null)
        {
            CurrentActiveMainLight.SetActive(false);
            CurrentActiveFillLight.SetActive(false);
        }

        CurrentActiveMainLight = MainLights[NewActiveLightIndex];
        CurrentActiveMainLight.SetActive(true);

        CurrentActiveFillLight = FillLights[NewActiveLightIndex];
        CurrentActiveFillLight.SetActive(true);
    }

    //A proper modulo, handling negatives as it should.
    private int TrueMod(int x, int m)
	{
        return (x % m + m) % m;
	}
}
