using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class HeatMapFileUtil 
{
    public static bool IsValidPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("Path is empty!");

            return false;
        }

        if (!File.Exists(path))
        {
            Debug.Log("File at given location doesn't exist!");

            return false;
        }

        return true;
    }

    private static bool CreateFile(string path)
    {
        if (File.Exists(path))
        {
            Debug.Log("File at given location already exists!");

            return false;
        }

        try
        {
            File.Create(path);
        }
        catch (System.Exception ex)
        {
           Debug.Log(ex.ToString());

           return false;
        }

        return true;
    }
    
    private static string[] ReadFile(string path)
    {
        if(IsValidPath(path))
        {
            try
            {
                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    return sr.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());

                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public static List<EventData> ReadVectorsFromFile(string path)
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
                EventData currentLineEvent;
                string[] properties = line.Split(';');

                if (properties.Length > 4)
                {
                    string nameOfEvent = properties[4];
                    Vector3 positionVector = new Vector3(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]));

                    // if event of current line is not in the events list than the new one should be created
                    if (!events.TryGetValue(nameOfEvent, out currentLineEvent))
                    {
                        currentLineEvent = new EventData();
                        currentLineEvent.name = properties[4];
                        events.Add(nameOfEvent, currentLineEvent);
                    }

                    Position position = new Position();
                    position.pos = positionVector;

                    currentLineEvent.positions.Add(position);
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
