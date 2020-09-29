using UnityEngine;

public interface ImpfxCallable 
{
	bool SetUp(GameObject Graphic, ModularPFXComponent OwningComponent);

	bool UpdatePFX();

	bool End( );
}
