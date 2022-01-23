using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
    public Vector3 pos;

    // points that have same (or almost same) position will be merged into one (during reading form file) for performance during calculations
    public int multiplier = 1; 
}
