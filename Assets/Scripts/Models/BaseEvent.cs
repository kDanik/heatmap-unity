using UnityEngine;

/// <summary>
/// Base event is used to save and load(read) data for heatmap.
/// This class can be extended with additional data, if required.
/// </summary>
[System.Serializable]
class BaseEvent
{
    /// <summary>
    /// Position of event in world space
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// Descriptive and unique name of event (example: User Position, 
    /// </summary>
    public string EventName;

    public BaseEvent(string EventName, Vector3 Position)
    {
        this.Position = Position;
        this.EventName = EventName;
    }
}
