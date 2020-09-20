using UnityEngine;

public interface ImpfxCallable 
{
	bool SetUp(MPFXContext Context, GameObject Graphic );

	bool UpdatePFX(MPFXContext Context);

	bool End(MPFXContext Context);

	abstract MPFXContext ConstructContext();
}
