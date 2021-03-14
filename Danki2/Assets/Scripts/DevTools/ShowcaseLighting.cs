using System.Collections.Generic;
using UnityEngine;

public class ShowcaseLighting : MonoBehaviour
{
    public List<GameObject> MainLights;
    public List<GameObject> FillLights;

    // Start is called before the first frame update
    void OnValidate()
    {
        if (MainLights.Count != FillLights.Count)
        {
            Debug.LogError("The number of main lights is not equal to the number of fill lights.");
            return;
        }

        if (MainLights.Count == 0)
        {
            Debug.LogError("No lights defined.");
            return;
        }
    }
    public int CurrentLightIndex => MainLights.FindIndex(l => l.activeInHierarchy);

    public void NextLight()
    {
        int index = TrueMod(CurrentLightIndex + 1, MainLights.Count);
        SetActiveLight(index);
	}

    public void PrevLight()
    {
        int index = TrueMod(CurrentLightIndex - 1, MainLights.Count);
        SetActiveLight(index);
    }

    public void HomeLight() => SetActiveLight(0);

    private void SetActiveLight(int index)
	{
        MainLights.ForEach(l => l.SetActive(false));
        FillLights.ForEach(l => l.SetActive(false));

        MainLights[index].SetActive(true);
        FillLights[index].SetActive(true);
    }

    //A proper modulo, handling negatives as it should.
    private int TrueMod(int x, int m)
	{
        return (x % m + m) % m;
	}
}
