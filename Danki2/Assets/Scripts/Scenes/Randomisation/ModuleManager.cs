using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleManager : Singleton<ModuleManager>
{
    private void Start()
    {
        Random.InitState(PersistenceManager.Instance.SaveData.CurrentRoomSaveData.ModuleSeed);

        List<ModuleSocket> sockets = FindObjectsOfType<ModuleSocket>().ToList();
        sockets.Sort();

        if (!sockets.DistinctById())
        {
            Debug.LogError("Invalid socket ids, ensure all socket ids are unique.");
            return;
        }

        InstantiateModules(sockets);
    }

    private void InstantiateModules(List<ModuleSocket> sockets)
    {
        sockets.ForEach(socket =>
        {
            List<GameObject> prefabsWithTags = ModuleLookup.Instance.GetPrefabsWithTags(socket.SocketType, socket.Tags);

            if (prefabsWithTags.Count == 0)
            {
                Debug.LogError($"Socket found with tags that match no modules, ensure socket {socket.Id} has valid tags.");
                return;
            }

            GameObject prefab = RandomUtils.Choice(prefabsWithTags);
            Instantiate(prefab, socket.transform);
        });
    }
}
