using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseLighting : MonoBehaviour
{

    public List<GameObject> Lights;

    private GameObject CurrentActiveLight = null;
    private bool IsVolumetricsEnabled = false;

    // Start is called before the first frame update
    void OnValidate()
    {
        if(Lights.Count > 0)
		{
            CurrentActiveLight = Lights[0];
            CurrentActiveLight.SetActive(true);
		}

        foreach (GameObject Light in Lights)
		{
            if( !Object.ReferenceEquals(Light, CurrentActiveLight))
			{
                Light.SetActive(false);
			}
		}
    }

    public void NextLight()
	{
        int index = Lights.IndexOf(CurrentActiveLight);
        index = ++index % Lights.Count;
        SetActiveLight(index);
	}

    public void PrevLight()
	{
        int index = Lights.IndexOf(CurrentActiveLight);
        index = TrueMod(--index, Lights.Count);
        SetActiveLight(index);
	}

    public void HomeLight()
	{
        SetActiveLight(0);
	}

    private void SetActiveLight(int NewActiveLightIndex)
	{
        CurrentActiveLight.SetActive(false);
        CurrentActiveLight = Lights[NewActiveLightIndex];
        CurrentActiveLight.SetActive(true);
	}

    //A proper modulo, handling negatives as it should.
    private int TrueMod(int x, int m)
	{
        return (x % m + m) % m;
	}
}
