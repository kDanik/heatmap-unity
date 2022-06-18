
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// This is just an example of EventWriter
public class CSVEventWriter : IEventWriter
{
    // If true, if file wasn't found for given path creates new one
    private readonly bool createFileIfNonFound;

    // path to existing file or path where new file should be created
    private readonly string path;

    private readonly bool hasFileToWrite;

    public CSVEventWriter(string path, bool createFileIfNonFound)
    {
        this.path = path;
        this.createFileIfNonFound = createFileIfNonFound;
        hasFileToWrite = Startup();
    }


    // tries to find or create file, on success returns true, otherwise false
    public bool Startup()
    {
        if (File.Exists(path)) return true;

        if (!createFileIfNonFound) return false;

        return CreateFile(path);
    }

    bool IEventWriter.WriterIsAvailable()
    {
        return hasFileToWrite;
    }


    /* 
        Saves one event to CSV-File. Returns true if successfully saved
    */
    bool IEventWriter.SaveEventInstance(BaseEvent baseEvent)
    {
        if (!hasFileToWrite || !File.Exists(path))
        {
            return false;
        }

        StreamWriter writer = new StreamWriter(path, true);

        try
        {
            writer.WriteLine(ComposeCSVLine(baseEvent));

            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());

            return false;
        }
        finally
        {
            writer.Close();
        }
    }


    /* 
        Saves list events to CSV-File.  Returns true every event is successfully saved
    */
    bool IEventWriter.SaveEventInstances(List<BaseEvent> baseEvents)
    {
        if (!hasFileToWrite || !File.Exists(path))
        {
            return false;
        }

        StreamWriter writer = new StreamWriter(path, true);

        try
        {
            foreach (BaseEvent baseEvent in baseEvents)
            {
                writer.WriteLine(ComposeCSVLine(baseEvent));
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());

            return false;
        }
        finally
        {
            writer.Close();
        }
    }





    private bool CreateFile(string path)
    {
        if (!createFileIfNonFound) return false;

        try
        {
            File.Create(path);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());

            return false;
        }
    }

    private string ComposeCSVLine(BaseEvent baseEvent)
    {
        return (baseEvent.eventName + ";" + baseEvent.position.x + ";" + baseEvent.position.y + ";" + baseEvent.position.z);
    }
}
