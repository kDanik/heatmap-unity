using System.Collections.Generic;

interface IEventWriter
{
    /// <summary>
    /// Saves one BaseEvent 
    /// </summary>
    /// <returns>true if data was saved successfully</returns>
    public bool SaveEvent(BaseEvent baseEvent);

    /// <summary>
    /// Saves list of BaseEvent-s (useful if data should be transported in bigger quantity, for example for REST-services or database batch save)
    /// </summary>
    /// <returns>true if data was saved successfully</returns>
    public bool SaveEvents(List<BaseEvent> baseEvents);

    /// <summary>
    /// Checks and returns availability status of writer
    /// </summary>
    /// <returns>true if writer is available for writing (valid configuration, initialized and etc), otherwise false</returns>
    public bool IsWriterAvailable();
}
