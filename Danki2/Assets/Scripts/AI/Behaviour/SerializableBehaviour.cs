using System;
using UnityEngine;

[Serializable]
public class SerializableBehaviour : ISerializationCallbackReceiver
{
    public Behaviour behaviour;

    [SerializeField]
    private SerializableType _serializableBehaviourType;

    [SerializeField]
    private float[] _args;

    public SerializableBehaviour()
    {
        behaviour = new NoOpBehaviour();
        _serializableBehaviourType = new SerializableType(typeof(NoOpBehaviour));
        _args = new float[0];
    }

    public SerializableBehaviour(Type behaviourType, float[] args)
    {
        _serializableBehaviourType = new SerializableType(behaviourType);
        _args = args;

        InitializeBehaviour();
    }

    public void OnAfterDeserialize()
    {
        InitializeBehaviour();
    }

    private void InitializeBehaviour()
    {
        behaviour = (Behaviour)Activator.CreateInstance(_serializableBehaviourType.Type);
        behaviour.Initialise(_args);
    }

    public void OnBeforeSerialize()
    {
    }
}
