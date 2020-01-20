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
            .Select(m => new MortalCacheItem(m, m.GetComponent<Collider>()))
            .ToList();
    }
}
