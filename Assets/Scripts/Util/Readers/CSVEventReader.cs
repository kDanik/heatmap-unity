using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVEventReader : IEventReader
{
    private readonly string path;

    private readonly bool hasFileToRead;

    public CSVEventReader(string path)
    {
        this.path = path;
        hasFileToRead = Startup();
    }

    public bool Startup()
    {
        if (File.Exists(path))
        {
            return true;
        }
        else {
            Debug.LogError("Invalid path, no file found: " + path);
            return false;
        }
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
                string[] properties = line.Split(';');

                if (properties.Length > 3)
                {
                    AddPropertiesDataToEventData(properties, events);
                }
                else
                {
                    Debug.Log("line is invalid : " + line);
                }
            }
        }

        return new List<EventData>(events.Values);
    }


    private void AddPropertiesDataToEventData(string[] properties, Dictionary<string, EventData> events)
    {
        Vector3 positionVector = new Vector3(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]));
        string nameOfEvent = properties[0];

        // if event of current event name is not in the EventData list than the new one EventData should be created
        if (!events.TryGetValue(nameOfEvent, out EventData currentLineEvent))
        {
            currentLineEvent = new EventData();
            currentLineEvent.name = nameOfEvent;
            events.Add(nameOfEvent, currentLineEvent);
        }

        EventPosition eventPosition = new EventPosition();
        eventPosition.positionVector = positionVector;

        currentLineEvent.positions.Add(eventPosition);
    }
}
