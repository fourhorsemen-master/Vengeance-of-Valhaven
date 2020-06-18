using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MPFXSettings
{
	public GameObject effectObject;

	[ColorUsage(true, true)]
	public Color effectColor;

	[ColorUsage(true, true)]
	public Color effectEmissive;
}
