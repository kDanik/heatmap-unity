using UnityEngine;

public class EventPosition
{
    public Vector3 positionVector;

    /* 
        EventPosition-s that have same (or almost same) positionVector-s will be merged into one EventPosition,
        with increasement of positionMultiplier
    */
    public int positionMultiplier = 1;
}
