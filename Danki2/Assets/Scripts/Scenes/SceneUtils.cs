using UnityEngine.SceneManagement;
using UnityScene = UnityEngine.SceneManagement.Scene;

public static class SceneUtils
{
    public static void LoadScene(Scene scene) => SceneManager.LoadScene(SceneLookup.Instance.GetFileName(scene));

    public static Scene FromUnityScene(UnityScene unityScene) => EnumUtils.FromString<Scene>(unityScene.name);
}
