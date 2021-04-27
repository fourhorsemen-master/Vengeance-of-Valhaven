using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneLookup : Singleton<SceneLookup>
{
   [SerializeField, HideInInspector]
   public SceneDataLookup sceneDataLookup = new SceneDataLookup(() => new SceneData());

   protected override bool DestroyOnLoad => false;

   public virtual Pole GetEntranceSide(Scene scene, int entranceId) => sceneDataLookup[scene].GameplaySceneData.EntranceData[entranceId].Side;

   public string GetFileName(Scene scene) => sceneDataLookup[scene].FileName;

   public List<Scene> GetValidScenes(RoomType roomType, Pole trueEntranceDirection, int numberOfExits)
   {
      return sceneDataLookup.Keys
         .Where(scene => sceneDataLookup[scene].SceneType == SceneType.Gameplay)
         .Where(scene => sceneDataLookup[scene].GameplaySceneData.RoomTypes.Contains(roomType))
         .Where(scene => GetValidCameraOrientations(scene, trueEntranceDirection, numberOfExits).Count > 0)
         .ToList();
   }

   public List<Pole> GetValidCameraOrientations(Scene scene, Pole trueEntranceDirection, int numberOfExits)
   {
      GameplaySceneData gameplaySceneData = sceneDataLookup[scene].GameplaySceneData;
      List<Pole> validCameraOrientations = new List<Pole>();

      EnumUtils.ForEach<Pole>(cameraOrientation =>
      {
         if (!gameplaySceneData.CameraOrientations.Contains(cameraOrientation)) return;

         // given this camera orientation, check if there is an entrance from the true entrance direction
         bool hasValidEntrance = gameplaySceneData.EntranceData
            .Any(entrance => OrientationUtils.GetTrueDirection(cameraOrientation, entrance.Side) == trueEntranceDirection);
         if (!hasValidEntrance) return;

         // given this camera orientation, check if there are enough exits
         //   - exits need to be on a different side to the entrance
         //   - exits cannot be facing south given the camera orientation
         int validExitCount = 0;
         gameplaySceneData.ExitData.ForEach(exit =>
         {
            Pole trueExitDirection = OrientationUtils.GetTrueDirection(cameraOrientation, exit.Side);

            if (trueExitDirection == trueEntranceDirection) return;

            if (trueExitDirection == Pole.South) return;

            validExitCount++;
         });

         if (validExitCount < numberOfExits) return;

         validCameraOrientations.Add(cameraOrientation);
      });

      return validCameraOrientations;
   }

   public List<int> GetValidEntranceIds(Scene scene, Pole trueEntranceDirection, Pole cameraOrientation)
   {
      List<int> validEntranceIds = new List<int>();

      Pole requiredEntranceSide = OrientationUtils.GetOriginalDirection(cameraOrientation, trueEntranceDirection);
      
      sceneDataLookup[scene].GameplaySceneData.EntranceData.ForEach(entranceData =>
      {
         if (entranceData.Side == requiredEntranceSide) validEntranceIds.Add(entranceData.Id);
      });

      return validEntranceIds;
   }

   public List<int> GetValidExitIds(Scene scene, Pole cameraOrientation, int entranceId)
   {
      List<int> validExits = new List<int>();

      Pole trueEntranceDirection = GetTrueEntranceDirection(scene, cameraOrientation, entranceId);
      
      sceneDataLookup[scene].GameplaySceneData.ExitData.ForEach(exitData =>
      {
         Pole trueExitDirection = OrientationUtils.GetTrueDirection(cameraOrientation, exitData.Side);
         if (trueExitDirection != trueEntranceDirection && trueExitDirection != Pole.South) validExits.Add(exitData.Id);
      });
      
      return validExits;
   }

   public Pole GetTrueExitDirection(Scene scene, Pole cameraOrientation, int exitId)
   {
      Pole exitSide = sceneDataLookup[scene]
         .GameplaySceneData
         .ExitData
         .First(e => e.Id == exitId)
         .Side;
      return OrientationUtils.GetTrueDirection(cameraOrientation, exitSide);
   }

   public Pole GetTrueEntranceDirection(Scene scene, Pole cameraOrientation, int entranceId)
   {
      Pole entranceSide = sceneDataLookup[scene]
         .GameplaySceneData
         .EntranceData
         .First(e => e.Id == entranceId)
         .Side;
      return OrientationUtils.GetTrueDirection(cameraOrientation, entranceSide);
   }
}
