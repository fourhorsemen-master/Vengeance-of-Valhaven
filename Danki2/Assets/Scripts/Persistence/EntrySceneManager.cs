using UnityEngine;

public class EntrySceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneUtils.LoadScene(Scene.MainMenu);
    }
}
