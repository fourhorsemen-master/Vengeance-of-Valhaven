using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public List<MortalCacheItem> MortalCache { get; private set; }

    private void Start()
    {
        Mortal[] mortals = FindObjectsOfType<Mortal>();
        MortalCache = mortals
            .Select(mortal =>
            {
                if (!mortal.TryGetComponent(out Collider collider))
                {
                    Debug.LogError($"Found mortal, of type {mortal.GetType()}, without a collider");
                    return null;
                }

                return new MortalCacheItem(mortal, collider);
            })
            .Where(i => i != null)
            .ToList();
    }
}
