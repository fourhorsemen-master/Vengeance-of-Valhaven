using UnityEngine;

public interface ImpfxCallable 
{
<<<<<<< HEAD
	bool SetUp(MPFXContext Context, GameObject Graphic );
=======
	bool SetUp(GameObject Graphic, ModularPFXComponent OwningComponent);
>>>>>>> master

	bool UpdatePFX(MPFXContext Context);

	bool End(MPFXContext Context);

	MPFXContext ConstructContext();
}
