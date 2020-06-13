using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInsertionArea : MonoBehaviour
{
    public Subject MouseUpSubject = new Subject();

    public void OnMouseUp()
    {
        MouseUpSubject.Next();
    }
}
