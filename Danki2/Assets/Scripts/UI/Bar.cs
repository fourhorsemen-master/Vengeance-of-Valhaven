using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    protected void SetWidth(float width)
    {
        transform.localScale = new Vector3(width, 1f, 1f);
    }
}
