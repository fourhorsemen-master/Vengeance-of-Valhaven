using System.Collections.Generic;
using System.Linq;

public static class Layers
{
    public static int Default => 0;
    public static int TransparentFX => 1;
    public static int IgnoreRaycast => 2;
    public static int Water => 4;
    public static int UI => 5;
    public static int Ethereal => 8;
    public static int Floor => 9;
    public static int Actors => 10;
    public static int Props => 11;
    public static int Abilities => 12;

    /// <summary>
    /// Returns a layer mask, which includes the given layers. This is an integer that, when in binary format,
    /// represents the layer mask. Each bit corresponds to a layer and is 1 iff the layer is in the mask.
    /// </summary>
    /// <param name="layers"> The layers to include in the mask </param>
    /// <returns> The layer mask </returns>
    public static int GetLayerMask(IEnumerable<int> layers)
    {
        return layers.Aggregate(0, (current, layer) => current | 1 << layer);
    }

    /// <summary>
    /// Inverts the given layer mask, so that all layers that are included in the given layer mask are not
    /// included in the returned layer mask, and vise versa.
    /// </summary>
    /// <param name="layerMask"> The layer mask to invert </param>
    /// <returns> The inverted layer mask </returns>
    public static int GetInvertedLayerMask(int layerMask)
    {
        return ~layerMask;
    }
}
