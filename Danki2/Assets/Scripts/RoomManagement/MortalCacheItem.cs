using UnityEngine;

public class MortalCacheItem
{
    public Mortal Mortal { get; }
    public Collider Collider { get; }

    public MortalCacheItem(Mortal mortal, Collider collider)
    {
        Mortal = mortal;
        Collider = collider;
    }
}
