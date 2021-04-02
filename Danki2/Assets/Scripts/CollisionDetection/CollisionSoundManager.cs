using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionSoundManager : Singleton<CollisionSoundManager>
{
    [SerializeField]
    [EventRef]
    private string collisionEvent = null;

    [SerializeField] private PhysicMaterial dirtPhysicMaterial = null;
    [SerializeField] private PhysicMaterial stonePhysicMaterial = null;
    [SerializeField] private PhysicMaterial waterPhysicMaterial = null;
    [SerializeField] private PhysicMaterial fleshPhysicMaterial = null;
    [SerializeField] private PhysicMaterial woodPhysicMaterial = null;

    private Dictionary<PhysicMaterial, MaterialParameterValue> physicMaterialNameToParameterValue;

    private static readonly List<MaterialParameterValue> descendingMaterialPriority = new List<MaterialParameterValue>
    {
        MaterialParameterValue.Flesh,
        MaterialParameterValue.Stone,
        MaterialParameterValue.Wood,
        MaterialParameterValue.Water,
        MaterialParameterValue.Dirt
    };

    private void Start()
    {
        physicMaterialNameToParameterValue = new Dictionary<PhysicMaterial, MaterialParameterValue>
        {
            [dirtPhysicMaterial] = MaterialParameterValue.Dirt,
            [stonePhysicMaterial] = MaterialParameterValue.Stone,
            [waterPhysicMaterial] = MaterialParameterValue.Water,
            [fleshPhysicMaterial] = MaterialParameterValue.Flesh,
            [woodPhysicMaterial] = MaterialParameterValue.Wood,
        };
    }

    public void Play(PhysicMaterial sharedMaterial, CollisionSoundLevel collisionSoundLevel)
    {
        Play(new HashSet<PhysicMaterial> { sharedMaterial }, collisionSoundLevel);
    }

    public void Play(ISet<PhysicMaterial> sharedMaterials, CollisionSoundLevel collisionSoundLevel)
    {
        List<MaterialParameterValue> materialParameterValues = sharedMaterials
            .Where(m => m != null)
            .Where(m => physicMaterialNameToParameterValue.ContainsKey(m))
            .Select(m => physicMaterialNameToParameterValue[m])
            .OrderBy(m => descendingMaterialPriority.IndexOf(m))
            .ToList();

        if (materialParameterValues.Count == 0) return;

        EventInstance eventInstance = RuntimeManager.CreateInstance(collisionEvent);
        eventInstance.setParameterByName("material", (int)materialParameterValues[0]);
        eventInstance.setParameterByName("size", (int)collisionSoundLevel);
        eventInstance.start();
        eventInstance.release();
    }

    private enum MaterialParameterValue
    {
        Dirt = 0,
        Stone = 1,
        Water = 2,
        Flesh = 3,
        Wood = 4
    }
}
