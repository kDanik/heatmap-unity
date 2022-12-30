using System.Collections.Generic;

interface IEventReader
{
    /// <summary>
    /// Reads event data and returns it as list of EventData objects
    /// </summary>
    public List<EventData> ReadEvents();

    /// <summary>
    /// Checks and returns availability status of reader
    /// </summary>
    /// <returns>true if reader is available for reading (valid, initialized and etc), otherwise false</returns>
    public bool ReaderIsAvailable();
}
