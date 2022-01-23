using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EventData is collection of tracked positions with specific tag.
// For example it could be "Logouts","Movement" and anything else

public class EventData
{
    public List<Position> positions = new List<Position>();

    public string name;
    public bool active = false;
}

