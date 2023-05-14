using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for classes, that record some event in some specified interval.
/// </summary>
public abstract class AbstractEventIntervalRecorder : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Interval for event recording in seconds")]
    private float interval = 0.5F;
    [SerializeField]
    [Tooltip("Is recording of event activate")]
    protected bool record = true;

    /// <summary>
    /// Coroutine that calls RecordAndSaveEvent() in specified interval
    /// </summary>
    protected IEnumerator StartEventRecording()
    {
        // small delay is required, to prevent "Sharing violation" for newly created files
        yield return new WaitForEndOfFrame();

        StartCoroutine(RecordEventInInterval());
    }


    protected IEnumerator RecordEventInInterval()
    {
        if (record)
        {
            RecordAndSaveEvent();

            yield return new WaitForSeconds(interval);

            StartCoroutine(RecordEventInInterval());
        }
        else
        {
            yield return null;
        }
    }

    /// <summary>
    /// Implementation of record and save functionality for some event
    /// </summary>
    protected abstract void RecordAndSaveEvent();
}
