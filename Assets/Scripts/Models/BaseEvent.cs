using UnityEngine;

class BaseEvent
{
    public readonly Vector3 position;
    public readonly string eventName;

    public BaseEvent(string eventName, Vector3 position)
    {
        this.position = position;
        this.eventName = eventName;
    }
}
