using UnityEngine;

public class ObjectPositionRecorder : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToRecord = null;
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



    void Start()
    {
        if (recordEvents == false)
        {
            return;
        }


        if (!IsObjectOnScene())
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
                RecordGameobjectPosition();

                timer = 0F;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void RecordGameobjectPosition()
    {
        BaseEvent baseEvent = PrepareData();

        eventWriter.SaveEventInstance(baseEvent);
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
