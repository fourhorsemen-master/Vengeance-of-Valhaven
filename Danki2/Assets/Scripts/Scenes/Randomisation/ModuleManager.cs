using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ModuleManager : Singleton<ModuleManager>
{
    private float yRotation;

    private static readonly Dictionary<Pole, float> orientationToYRotation = new Dictionary<Pole, float>
    {
        [Pole.North] = 0,
        [Pole.East] = 90,
        [Pole.South] = 180,
        [Pole.West] = 270
    };

    private void Start()
    {
        RoomSaveData currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        Random.InitState(currentRoomSaveData.ModuleSeed);
        yRotation = orientationToYRotation[currentRoomSaveData.CameraOrientation];

        List<ModuleSocket> sockets = FindObjectsOfType<ModuleSocket>().ToList();
        sockets.SortById();

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
            socket.transform.Rotate(0, yRotation, 0);

            List<ModuleData> moduleDataList = ModuleLookup.Instance.GetModuleDataWithMatchingTags(socket.SocketType, socket.Tags);

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
                AddRandomFreeRotation(module, moduleData.MinFreeRotation, moduleData.MaxFreeRotation);
                break;
            case SocketRotationType.Distinct:
                AddRandomDistinctRotation(module, moduleData.DistinctRotations);
                break;
        }
    }

    private void AddRandomFreeRotation(GameObject module, float min, float max)
    {
        module.transform.Rotate(0, Random.Range(min, max), 0);
    }

    private void AddRandomDistinctRotation(GameObject module, List<float> rotations)
    {
        if (rotations.Count == 0) return;
        module.transform.Rotate(0, RandomUtils.Choice(rotations), 0);
    }
}
