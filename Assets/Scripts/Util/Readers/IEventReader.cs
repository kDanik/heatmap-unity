using System.Collections.Generic;

interface IEventReader
{
    /* 
        reads data and returns dictionary of EventData objects
    */
    public List<EventData> ReadEvents();

    /* 
         Returns true if reader is ready to use and valid 
    */
    public bool ReaderIsAvailable();
}
