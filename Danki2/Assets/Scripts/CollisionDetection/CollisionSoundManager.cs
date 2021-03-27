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

    public void Play(IEnumerable<PhysicMaterial> materials, CollisionSoundLevel collisionSoundLevel, Vector3 position)
    {
        if (!materials.Any()) return;

        EventInstance eventInstance = RuntimeManager.CreateInstance(collisionEvent);
        eventInstance.setParameterByName("material", 0);
        eventInstance.setParameterByName("size", (int)collisionSoundLevel);
        eventInstance.start();
        eventInstance.release();
    }
}
