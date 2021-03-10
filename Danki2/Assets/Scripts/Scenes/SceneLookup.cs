public class SceneLookup : NotDestroyedOnLoadSingleton<SceneLookup>
{
   public SceneDataLookup sceneDataLookup = new SceneDataLookup(() => new SceneData());

   public string GetFileName(Scene scene) => sceneDataLookup[scene].FileName;
}
