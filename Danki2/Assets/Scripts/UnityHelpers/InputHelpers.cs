using UnityEngine;

public static class InputHelpers
{
    public static ScreenQuadrant GetMouseScreenQuadrant()
    {
        var top = Input.mousePosition.y < Screen.height / 2;
        var left = Input.mousePosition.x < Screen.width / 2;

        return top
            ? left ? ScreenQuadrant.TopLeft : ScreenQuadrant.TopRight
            : left ? ScreenQuadrant.BottomLeft : ScreenQuadrant.BottomRight;
    }
}
