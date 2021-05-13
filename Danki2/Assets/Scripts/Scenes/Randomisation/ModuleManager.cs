using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ModuleManager : Singleton<ModuleManager>
{
    private void Start()
    {
        RoomNode currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;
        Random.InitState(currentRoomNode.ModuleSeed);

        List<ModuleSocket> sockets = FindObjectsOfType<ModuleSocket>().ToList();
        sockets.SortById();

        if (!sockets.IsDistinctById())
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
            List<ModuleData> moduleDataList = ModuleLookup.Instance.GetModuleDataWithMatchingTags(
                socket.SocketType,
                socket.Tags,
                socket.TagsToExclude
            );

            if (moduleDataList.Count == 0)
            {
                Debug.LogError($"Socket found with tags that match no modules, ensure socket {socket.Id} has valid tags.");
                return;
            }

            ModuleData moduleData = RandomUtils.Choice(moduleDataList);
            GameObject module = Instantiate(moduleData.Prefab, socket.transform);
            AddRandomRotation(module, moduleData, ModuleLookup.Instance.GetSocketRotationType(socket.SocketType));
        });
    }

    private void AddRandomRotation(GameObject module, ModuleData moduleData, SocketRotationType socketRotationType)
    {
        switch (socketRotationType)
        {
            case SocketRotationType.Free:
                AddRandomFreeRotation(module, moduleData.AllowAnyFreeRotation, moduleData.MinFreeRotation, moduleData.MaxFreeRotation);
                break;
            case SocketRotationType.Distinct:
                AddRandomDistinctRotation(module, moduleData.DistinctRotations);
                break;
        }
    }

    private void AddRandomFreeRotation(GameObject module, bool allowAnyRotation, float min, float max)
    {
        float rotation = allowAnyRotation
            ? Random.Range(0, 360)
            : Random.Range(min, max);

        module.transform.Rotate(0, rotation, 0);
    }

    private void AddRandomDistinctRotation(GameObject module, List<float> rotations)
    {
        if (rotations.Count == 0) return;
        module.transform.Rotate(0, RandomUtils.Choice(rotations), 0);
    }
}
