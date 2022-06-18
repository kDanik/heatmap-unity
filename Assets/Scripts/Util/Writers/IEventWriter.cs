using System.Collections.Generic;

interface IEventWriter
{
    /* 
        Saves ones BaseEvent
    */
    public bool SaveEventInstance(BaseEvent baseEvent);


    /* 
         Saves list of BaseEvent-s (useful if data should be transported bigger ammounts, for example for REST-service)
    */
    public bool SaveEventInstances(List<BaseEvent> baseEvents);

    /* 
         Returns true if writer is ready to use and valid 
    */
    public bool WriterIsAvailable();
}
