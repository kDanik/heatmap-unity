using UnityEngine;

[System.Serializable]
class BaseEvent
{
    public Vector3 position;
    public string eventName;

    public BaseEvent(string eventName, Vector3 position)
    {
        this.position = position;
        this.eventName = eventName;
    }
}
