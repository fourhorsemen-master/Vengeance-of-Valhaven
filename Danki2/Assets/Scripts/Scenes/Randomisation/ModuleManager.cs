using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleManager : Singleton<ModuleManager>
{
    // private void Start()
    // {
    //     Random.InitState(PersistenceManager.Instance.SaveData.CurrentRoomSaveData.ModuleSeed);
    //
    //     List<ModuleSocket> sockets = FindObjectsOfType<ModuleSocket>().ToList();
    //     sockets.Sort();
    //     
    //     sockets.ForEach(socket =>
    //     {
    //         List<GameObject> prefabsWithTags = ModuleLookup.Instance.GetPrefabsWithTags(socket.SocketType, socket.Tags);
    //         GameObject prefab = RandomUtils.Choice(prefabsWithTags);
    //         Instantiate(prefab, socket.transform);
    //     });
    // }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) Regenerate();
    }

    private void Regenerate()
    {
        List<ModuleSocket> sockets = FindObjectsOfType<ModuleSocket>().ToList();
        sockets.Sort();

        sockets.ForEach(socket =>
        {
            foreach (Transform t in socket.transform) Destroy(t.gameObject);

            List<GameObject> prefabsWithTags = ModuleLookup.Instance.GetPrefabsWithTags(socket.SocketType, socket.Tags);
            GameObject prefab = RandomUtils.Choice(prefabsWithTags);
            Instantiate(prefab, socket.transform);
        });
    }
}
