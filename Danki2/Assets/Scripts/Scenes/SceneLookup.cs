using System.Collections.Generic;
using UnityEngine;

public class SceneLookup : Singleton<SceneLookup>
{
   [SerializeField]
   public SceneDataLookup sceneDataLookup = new SceneDataLookup(() => new SceneData());

   protected override bool DestroyOnLoad => false;

   public string GetFileName(Scene scene) => sceneDataLookup[scene].FileName;

   public List<Scene> GetValidScenes(Pole entranceDirection, int numberOfExits)
   {
      return ListUtils.Singleton(Scene.RandomisedScene);
   }

   public List<Pole> GetValidCameraOrientations(Scene scene, Pole entranceDirection, int numberOfExits)
   {
      return new List<Pole>{Pole.North, Pole.East, Pole.South, Pole.West};
   }

   public List<int> GetValidEntranceIds(Scene scene, Pole entranceDirection, Pole cameraOrientation)
   {
      return new List<int>{0, 1};
   }

   public List<ExitData> GetValidExits(Scene scene, Pole cameraOrientation, int entranceId)
   {
      return new List<ExitData>
      {
         new ExitData { Id = 0, Side = Pole.West },
         new ExitData { Id = 1, Side = Pole.North },
         new ExitData { Id = 2, Side = Pole.East }
      };
   }

   public Pole GetTrueExitDirection(Scene scene, Pole cameraOrientation, int exitId)
   {
      return Pole.North;
   }
}
