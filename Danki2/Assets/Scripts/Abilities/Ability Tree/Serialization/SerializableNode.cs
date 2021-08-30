using System;
using UnityEngine;

[Serializable]
public class SerializableNode
{
    [SerializeField] private SerializableGuid abilityId;
    [SerializeField] private int id;
    [SerializeField] private int leftChildId;
    [SerializeField] private int rightChildId;
    
    public SerializableGuid AbilityId => abilityId;
    public int Id => id;
    public int LeftChildId => leftChildId;
    public int RightChildId => rightChildId;
    
    public SerializableNode(SerializableGuid abilityId, int id, int leftChildId, int rightChildId)
    {
        this.abilityId = abilityId;
        this.id = id;
        this.leftChildId = leftChildId;
        this.rightChildId = rightChildId;
    }
}
