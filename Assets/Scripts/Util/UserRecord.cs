using UnityEngine;
using System.IO;

public class UserRecord : MonoBehaviour
{

    public Camera mainCamera = null;
    public GameObject objectToRecord = null;
    [SerializeField]
    private float recordInterval = 0.5F;

    [SerializeField]
    private bool recordEvents = true;
    [SerializeField]
    public string dataPath;


    private StreamWriter writer;
    private Vector3 centerOfScreen = new Vector3(0.5F, 0.5F, 0.5F);
    private float timer = 0F;
    private bool recordObject;
    private bool recordCamera;
    private int sessionId;
    // private float afkTimer = 0F; TODO maybe implement it later



    void Start()
    {
        if(recordEvents == true)
        {
            sessionId = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;

            recordCamera = CheckCameraToRecord();
            recordObject = CheckObjectToRecord();
            recordEvents = CreateOrFindFile(dataPath) && (recordCamera || recordObject);
        }
    }

    private bool CheckCameraToRecord()
    {
        if (mainCamera == null)
        {
            mainCamera = objectToRecord.GetComponent<Camera>();

            return mainCamera != null;
        }
        else
        {           
            return true;
        }
    }

    private bool CheckObjectToRecord()
    {
        if (objectToRecord != null)
        {
            if (objectToRecord.scene.name != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    void Update()
    {
        if (recordEvents)
        {
            if (timer > recordInterval)
            {
                RecordPlayerData();
                
                timer = 0F;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    } 


    /*
        Records position and position of object that player is looking at
    */
    private void RecordPlayerData()
    {
        Ray ray = mainCamera.ViewportPointToRay(centerOfScreen);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50F , 1, QueryTriggerInteraction.Ignore))
        {
            WriteEventInFile(dataPath, "Player Look At", hit.point, sessionId);
        }
        WriteEventInFile(dataPath, "Player Position", mainCamera.transform.position, sessionId);
    }

    private void RecordGameobjectPosition()
    {
        WriteEventInFile(dataPath, "RecordSomeGameObjectPosition", objectToRecord.transform.position, sessionId);
    }






    /* 
        File utils (partly duplicate from HeatMapFileUtil, but makes sense for me to separate them)
    */

    private static bool CreateOrFindFile(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            try
            {
                File.Create(path);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());

                return false;
            }
        }

        return true;
    }

    /*
        can be accessed from other scripts if needed
        TODO : improve saving and make it more reliable + easy to implement different ways to save it ( some interface or smth)
    */

    public bool WriteEventInFile(string path, string name, Vector3 position, int sesssioId)
    {
        if (CreateOrFindFile(path))
        {
            StreamWriter writer = new StreamWriter(path, true);

            // that is too primitive and not reliable way to save it
            writer.WriteLine(sessionId + ";" + position.x + ";" + position.y + ";" + position.z + ";" + name);

            writer.Close();

            return true;
        }
        return false;
    }

}
