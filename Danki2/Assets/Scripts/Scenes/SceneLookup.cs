using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneLookup : Singleton<SceneLookup>
{
   [SerializeField]
   public SceneDataLookup sceneDataLookup = new SceneDataLookup(() => new SceneData());

   protected override bool DestroyOnLoad => false;

   private static readonly Dictionary<Pole, Dictionary<Pole, Pole>> cameraOrientationAndElementSideToTrueElementSide
      = new Dictionary<Pole, Dictionary<Pole, Pole>>
      {
         [Pole.North] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.North,
            [Pole.East] = Pole.East,
            [Pole.South] = Pole.South,
            [Pole.West] = Pole.West
         },
         [Pole.East] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.West,
            [Pole.East] = Pole.North,
            [Pole.South] = Pole.East,
            [Pole.West] = Pole.South
         },
         [Pole.South] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.South,
            [Pole.East] = Pole.West,
            [Pole.South] = Pole.North,
            [Pole.West] = Pole.East
         },
         [Pole.West] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.East,
            [Pole.East] = Pole.South,
            [Pole.South] = Pole.West,
            [Pole.West] = Pole.North
         }
      };

   private static readonly Dictionary<Pole, Dictionary<Pole, Pole>> cameraOrientationAndTrueElementSideToElementSide
      = new Dictionary<Pole, Dictionary<Pole, Pole>>
      {
         [Pole.North] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.North,
            [Pole.East] = Pole.East,
            [Pole.South] = Pole.South,
            [Pole.West] = Pole.West
         },
         [Pole.East] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.East,
            [Pole.East] = Pole.South,
            [Pole.South] = Pole.West,
            [Pole.West] = Pole.North
         },
         [Pole.South] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.South,
            [Pole.East] = Pole.West,
            [Pole.South] = Pole.North,
            [Pole.West] = Pole.East
         },
         [Pole.West] = new Dictionary<Pole, Pole>
         {
            [Pole.North] = Pole.West,
            [Pole.East] = Pole.North,
            [Pole.South] = Pole.East,
            [Pole.West] = Pole.South
         }
      };

   public string GetFileName(Scene scene) => sceneDataLookup[scene].FileName;

   public List<Scene> GetValidScenes(Pole trueEntranceDirection, int numberOfExits)
   {
      return sceneDataLookup.Keys
         .Where(scene => sceneDataLookup[scene].SceneType == SceneType.Gameplay)
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
            .Any(entrance => GetTrueEntranceDirection(scene, cameraOrientation, entrance.Id) == trueEntranceDirection);
         if (!hasValidEntrance) return;

         // given this camera orientation, check if there are enough exits
         //   - exits need to be on a different side to the entrance
         //   - exits cannot be facing south given the camera orientation
         int validExitCount = 0;
         gameplaySceneData.ExitData.ForEach(exit =>
         {
            Pole trueExitDirection = GetTrueExitDirection(scene, cameraOrientation, exit.Id);

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

      Pole requiredEntranceSide = cameraOrientationAndTrueElementSideToElementSide[cameraOrientation][trueEntranceDirection];
      
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
         Pole trueExitDirection = GetTrueExitDirection(scene, cameraOrientation, exitData.Id);
         if (trueExitDirection != trueEntranceDirection && trueExitDirection != Pole.South) validExits.Add(exitData.Id);
      });
      
      return validExits;
   }

   public Pole GetTrueExitDirection(Scene scene, Pole cameraOrientation, int exitId)
   {
      Pole exitSide = sceneDataLookup[scene].GameplaySceneData.ExitData.First(e => e.Id == exitId).Side;
      return cameraOrientationAndElementSideToTrueElementSide[cameraOrientation][exitSide];
   }

   public Pole GetTrueEntranceDirection(Scene scene, Pole cameraOrientation, int entranceId)
   {
      Pole entranceSide = sceneDataLookup[scene].GameplaySceneData.EntranceData.First(e => e.Id == entranceId).Side;
      return cameraOrientationAndElementSideToTrueElementSide[cameraOrientation][entranceSide];
   }
}
