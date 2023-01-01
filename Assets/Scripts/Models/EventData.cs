using System.Collections.Generic;

/// <summary>
/// Container for event data (name, positions and status in heatmap), used for heatmap visualization
/// </summary>
public class EventData
{
    public List<MergedEventPosition> Positions = new();

    public string EventName;

    /// <summary>
    /// Is this event already used for heatmap color calculations
    /// </summary>
    public bool IsCurrentlyDisplayedOnHeatmap = false;
}

