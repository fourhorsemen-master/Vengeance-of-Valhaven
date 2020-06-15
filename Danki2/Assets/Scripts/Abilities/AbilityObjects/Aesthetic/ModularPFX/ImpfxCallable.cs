using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ImpfxCallable 
{
	bool SetUp(GameObject Graphic );

	bool UpdatePFX();

	bool End( );
}
