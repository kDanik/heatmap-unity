using System.Collections.Generic;

/// <summary>
/// Container for event data (name, positions and status in heatmap), used for heatmap visualization
/// </summary>
public class EventData
{
    public List<MergedEventPosition> Positions = new();

    public string EventName;

    /// <summary>
    /// Should this event be used for heatmap visualisation
    /// </summary>
    public bool ShouldEventBeVisualised = false;
}

