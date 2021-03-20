using System.Collections.Generic;

public static class OrientationUtils
{
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
   
   private static readonly Dictionary<Pole, float> orientationToYRotation = new Dictionary<Pole, float>
   {
      [Pole.North] = 0,
      [Pole.East] = 90,
      [Pole.South] = 180,
      [Pole.West] = 270
   };
   
   private static readonly Dictionary<Pole, Pole> poleToReversedPole = new Dictionary<Pole, Pole>
   {
      [Pole.North] = Pole.South,
      [Pole.East] = Pole.West,
      [Pole.South] = Pole.North,
      [Pole.West] = Pole.East,
   };
   
   public static Pole GetTrueDirection(Pole cameraOrientation, Pole side)
   {
      return cameraOrientationAndElementSideToTrueElementSide[cameraOrientation][side];
   }

   public static Pole GetOriginalDirection(Pole cameraOrientation, Pole trueSide)
   {
      return cameraOrientationAndTrueElementSideToElementSide[cameraOrientation][trueSide];
   }

   public static float GetYRotation(Pole cameraOrientation)
   {
      return orientationToYRotation[cameraOrientation];
   }

   public static Pole GetReversedPole(Pole pole)
   {
      return poleToReversedPole[pole];
   }
}
