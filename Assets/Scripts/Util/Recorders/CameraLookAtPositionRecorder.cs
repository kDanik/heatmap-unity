using UnityEngine;

/// <summary>
/// Example of event recorder, that tracks and saves camera look at position (Raycast) in specified interval.
/// </summary>
public class CameraLookAtPositionRecorder : AbstractEventIntervalRecorder
{
    [SerializeField]
    private Camera cameraToRecord = null;
    [SerializeField]
    private string dataPath;
    [SerializeField]
    private string eventName;
    [SerializeField]
    private bool createFileIfNonFound;

    private IEventWriter eventWriter;

    private readonly Vector3 centerOfScreen = new(0.5F, 0.5F, 0.5F);


    void Awake()
    {
        if (!record) return;

        if (!IsCameraOnTheScene())
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

        if (baseEvent == null) return;

        eventWriter.SaveEvent(baseEvent);
    }

    private BaseEvent PrepareData()
    {
        Ray ray = cameraToRecord.ViewportPointToRay(centerOfScreen);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50F, 1, QueryTriggerInteraction.Ignore))
        {
            return new BaseEvent(eventName, hit.point);
        }

        return null;
    }

    private bool IsCameraOnTheScene()
    {
        return cameraToRecord != null;
    }
}
