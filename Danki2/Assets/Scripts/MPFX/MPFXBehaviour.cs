using System;
using UnityEngine;

[Serializable]
public abstract class MPFXBehaviour : ScriptableObject
{
	virtual public void SetUp(MPFXContext context, GameObject inGraphic)
	{
		context.graphic = inGraphic;
		context.timeElapsed = 0f;
	}

	public bool UpdatePFX(MPFXContext context)
	{
		context.timeElapsed += Time.deltaTime;
		UpdateInternal(context);
		return context.timeElapsed > context.endTime;
	}

	protected abstract void UpdateInternal(MPFXContext context);

	virtual public MPFXContext ConstructContext()
	{
		return new MPFXContext();
	}
}
