using UnityEngine;

public class CameraLookAtPositionRecorder : MonoBehaviour
{
    [SerializeField]
    private Camera cameraToRecord = null;
    [SerializeField]
    private string dataPath;
    [SerializeField]
    private string eventName;
    [SerializeField]
    private bool createFileIfNonFound;
    [SerializeField]
    private float recordInterval = 0.5F;
    [SerializeField]
    private bool recordEvents = true;

    private IEventWriter eventWriter;

    private float timer = 0F;
    private Vector3 centerOfScreen = new Vector3(0.5F, 0.5F, 0.5F);


    void Start()
    {
        if (recordEvents == false)
        {
            return;
        }


        if (!IsCameraOnTheScene())
        {
            recordEvents = false;
            return;
        }

        eventWriter = new CSVEventWriter(dataPath, createFileIfNonFound);
        recordEvents = eventWriter.WriterIsAvailable();
    }

    void Update()
    {
        if (recordEvents)
        {
            if (timer > recordInterval)
            {
                RecordCameraLookAtPosition();

                timer = 0F;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void RecordCameraLookAtPosition()
    {
        BaseEvent baseEvent = PrepareData();

        if (baseEvent == null) return;

        eventWriter.SaveEventInstance(baseEvent);
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
