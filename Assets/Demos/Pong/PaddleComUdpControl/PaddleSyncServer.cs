using UnityEngine;




[System.Serializable]
public class PaddleState
{
    public Vector3 PositionPaddle;
}



public class PaddleSyncServer : MonoBehaviour
{

    
    public string ObjectId;

    
    ServerManager ServerMan;
    float NextUpdateTimeout = -1;


    void Awake()
    {
        if (!Globals.IsServer)
        {
            enabled = false;
        }

    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ServerMan = FindFirstObjectByType<ServerManager>();
    }

    // Update is called once per frame
    void Update()
    {

         if (Time.time > NextUpdateTimeout)
        {
            PaddleState state = new PaddleState
            {
                PositionPaddle = transform.position
            };

            string json = JsonUtility.ToJson(state);

            ServerMan.BroadcastUDPMessage("UPDATEPADDLE|"+ObjectId+"|"+ json);

            Debug.Log("//////////////////////////++++ SERVEUR ++++ //////////////////////////");
            Debug.Log("NEWVALUE PADLE SERVER Numero "+ObjectId+"-->>> is ::  "+ json);
            Debug.Log("//////////////////////////////////////////////////////////////////");

            NextUpdateTimeout = Time.time + 0.03f;
        }
        
    }
}
