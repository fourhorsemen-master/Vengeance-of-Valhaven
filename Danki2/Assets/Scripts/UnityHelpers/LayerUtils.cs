using System.Linq;

public static class LayerUtils
{
    /// <summary>
    /// Returns a layer mask, which includes the given layers. This is an integer that, when in binary format,
    /// represents the layer mask. Each bit corresponds to a layer and is 1 iff the layer is in the mask.
    /// </summary>
    /// <param name="layers"> The layers to include in the mask </param>
    /// <returns> The layer mask </returns>
    public static int GetLayerMask(params Layer[] layers)
    {
        return layers.Aggregate(0, (current, layer) => current | 1 << (int)layer);
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
