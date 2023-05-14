using UnityEngine;

/// <summary>
/// Example of event recorder, that tracks and saves object position in specified interval.
/// </summary>
public class ObjectPositionRecorder : AbstractEventIntervalRecorder
{
    [SerializeField]
    private GameObject objectToRecord = null;
    [SerializeField]
    private string dataPath;
    [SerializeField]
    private string eventName;
    [SerializeField]
    private bool createFileIfNonFound;

    private IEventWriter eventWriter;

    void Awake()
    {
        if (!record) return;

        if (!IsObjectOnScene())
        {
            record = false;
            return;
        }

        eventWriter = new JSONEventWriter(dataPath, createFileIfNonFound);
        record = eventWriter.IsWriterAvailable();

        if (record) StartCoroutine(StartEventRecording());
    }

    protected override void RecordAndSaveEvent()
    {
        BaseEvent baseEvent = PrepareData();

        eventWriter.SaveEvent(baseEvent);
    }


    private BaseEvent PrepareData()
    {
        return new BaseEvent(eventName, objectToRecord.transform.position);
    }

    private bool IsObjectOnScene()
    {
        if (objectToRecord == null) return false;

        return (objectToRecord.scene.name != null);
    }
}
