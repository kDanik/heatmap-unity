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
        return File.Exists(path);
    }

    bool IEventReader.ReaderIsAvailable()
    {
        return hasFileToRead;
    }

    List<EventData> IEventReader.ReadEvents()
    {
        var events = new Dictionary<string, EventData>();
        int count = 0;

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
                    Vector3 positionVector = new Vector3(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]));
                    string nameOfEvent = properties[0];

                    // if event of current line is not in the events list than the new one should be created
                    if (!events.TryGetValue(nameOfEvent, out EventData currentLineEvent))
                    {
                        currentLineEvent = new EventData();
                        currentLineEvent.name = nameOfEvent;
                        events.Add(nameOfEvent, currentLineEvent);
                    }

                    EventPosition eventPosition = new EventPosition();
                    eventPosition.positionVector = positionVector;

                    currentLineEvent.positions.Add(eventPosition);
                    count++;
                }
                else
                {
                    Debug.Log("line is invalid : " + line);
                }
            }
        }
        Debug.Log("Number of vectors - > " + count);

        return new List<EventData>(events.Values);
    }
}
