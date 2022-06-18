using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONEventReader : IEventReader
{
    private readonly string path;

    private readonly bool hasFileToRead;

    public JSONEventReader(string path)
    {
        this.path = path;
        hasFileToRead = Startup();
    }

    public bool Startup()
    {
        return File.Exists(path);
    }

    bool IEventReader.ReaderIsAvailable()
    {
        return hasFileToRead;
    }

    List<EventData> IEventReader.ReadEvents()
    {
        var events = new Dictionary<string, EventData>();

        using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (BufferedStream bs = new BufferedStream(fs))
        using (StreamReader sr = new StreamReader(bs))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                BaseEvent baseEvent = JsonUtility.FromJson<BaseEvent>(line);

                if (baseEvent.eventName != null)
                {
                    AddBaseEventToEventData(baseEvent, events);
                }
                else
                {
                    Debug.Log("line is invalid : " + line);
                }
            }
        }

        return new List<EventData>(events.Values);
    }


    private void AddBaseEventToEventData(BaseEvent baseEvent, Dictionary<string, EventData> events)
    {
        EventData currentLineEvent;

        // if event of current event name is not in the EventData list than the new one EventData should be created
        if (!events.TryGetValue(baseEvent.eventName, out currentLineEvent))
        {
            currentLineEvent = new EventData();
            currentLineEvent.name = baseEvent.eventName;
            events.Add(baseEvent.eventName, currentLineEvent);
        }

        EventPosition eventPosition = new EventPosition();
        eventPosition.positionVector = baseEvent.position;

        currentLineEvent.positions.Add(eventPosition);
    }
}
