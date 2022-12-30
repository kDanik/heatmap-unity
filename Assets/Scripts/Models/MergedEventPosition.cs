using UnityEngine;

/// <summary>
/// Container with position and multiplier, that represents other positions with same or almost same value that were merged into this object.
/// </summary>
public class MergedEventPosition
{
    public Vector3 Position;

    /// <summary>
    /// Positions that have same (or almost same) values will be merged into one EventPosition,
    /// by increasing positionMultiplier by 1.
    /// </summary>
    public int Multiplier = 1;
}
