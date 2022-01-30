using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EventData is collection of tracked positions with specific tag.

public class EventData
{
    public List<EventPosition> positions = new List<EventPosition>();

    public string name;
    public bool active = false;
}

