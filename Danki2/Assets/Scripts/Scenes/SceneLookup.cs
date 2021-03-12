public class SceneLookup : Singleton<SceneLookup>
{
   public SceneDataLookup sceneDataLookup = new SceneDataLookup(() => new SceneData());

   protected override bool DestroyOnLoad => false;

   public string GetFileName(Scene scene) => sceneDataLookup[scene].FileName;
}
