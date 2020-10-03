using System.Collections.Generic;
using UnityEngine;

public class Cluster
{
    private readonly List<WolfSpawner> spawners = new List<WolfSpawner>();

    public Cluster(WolfSpawner spawner)
    {
        spawners.Add(spawner);
    }

    public void AddSpawner(WolfSpawner spawner)
    {
        spawners.Add(spawner);
    }
    
    public Vector3 GetAveragePosition()
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
 
        foreach (WolfSpawner spawner in spawners)
        {
            Vector3 position = spawner.transform.position;
            x += position.x;
            y += position.y;
            z += position.z;
        }

        return new Vector3(x / spawners.Count, y / spawners.Count, z / spawners.Count);
    }

    public void Spawn(int wave)
    {
        for (int i = 0; i < wave; i++)
        {
            spawners[i % spawners.Count].Spawn();
        }
    }
}