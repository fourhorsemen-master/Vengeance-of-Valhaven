using UnityEngine;

public class GameplayEntrySceneManager : MonoBehaviour
{
    private void Start()
    {
        int id = PersistenceManager.Instance.SaveData.CurrentRoomId;
        Scene scene = PersistenceManager.Instance.SaveData.RoomSaveDataLookup[id].Scene;
        SceneUtils.LoadScene(scene);
    }
}
